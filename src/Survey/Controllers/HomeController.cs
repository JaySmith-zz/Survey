using System.Globalization;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Highway.Data;
using Survey.Core;
using Survey.Core.Domain;
using Survey.Core.Queries;
using Survey.Models;
using Survey.ViewModels;

namespace Survey.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IRepository repository, IHomeViewModelBuilder viewModelBuilder)
        {
            _repository = repository;
            _viewModelBuilder = viewModelBuilder;
        }

        private readonly IRepository _repository;
        private readonly IHomeViewModelBuilder _viewModelBuilder;

        [OutputCache(VaryByHeader = "*", Duration = 0, NoStore = true)]  // Disables Browser Cache for Action
        public ActionResult Index()
        {
            Query<Questionnaire> questionnaireQuery;

            if (User.IsInRole("Administrator") || User.IsInRole("Survey Manager"))
            {
                questionnaireQuery = new QuestionnaireAll();
            }
            else
            {
                questionnaireQuery = new QuestionnaireByUserId(User.Identity.Name);
            }

            var questionnaires = _repository.Find(questionnaireQuery).ToList();
            var model = _viewModelBuilder.GetHomeIndexModel(questionnaires);

            var items = Globals.QuestionnaireStatuses();

            ViewBag.Statuses = Globals.QuestionnaireStatuses();

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var query = new QuestionnaireById(id);
            var questionnaire = _repository.Find(query);

            if (questionnaire.Status == QuestionnaireStatus.NotStarted)
            {
                questionnaire.Status = QuestionnaireStatus.InProgress;
                questionnaire.ModifiedBy = User.Identity.Name.ToUpper();
                questionnaire.Modified = DateTime.Now;
                questionnaire.ModifiedBy = User.Identity.Name.ToUpper();
                questionnaire.Started = DateTime.Now;
                questionnaire.StartedBy = User.Identity.Name.ToUpper();

                _repository.Context.Update(questionnaire);
                _repository.Context.Commit();
            }

            var model = new HomeEditViewModel(_repository, questionnaire)
                {
                    DisplayLogicDepth = Globals.DisplayLogicDepth
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var id = int.Parse(collection["hidQuestionnaireId"]);

            var query = new QuestionnaireById(id);
            var questionnaire = _repository.Find(query);

            questionnaire.Status = QuestionnaireStatus.Complete;
            questionnaire.Modified = DateTime.Now;
            questionnaire.ModifiedBy = User.Identity.Name;

            _repository.Context.Update(questionnaire);
            _repository.Context.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult Export(int id)
        {
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });

            var query = new QuestionnaireById(id);
            var questionnaire = _repository.Find(query);
            var exporter = new QuestionnaireExcelExporter(questionnaire);

            var fileName = string.Format("{0} - {1}.xlsx", questionnaire.Survey.Name, questionnaire.QuestionniareName);

            return File(exporter.Export(), @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
        }

        public ActionResult Copy(int id)
        {
            var model = new HomeCopyViewModel();

            var sourceQuestionnaireQuery = new QuestionnaireById(id);
            var sourceQuestionnaire = _repository.Find(sourceQuestionnaireQuery);
            model.SourceQuestionnaire = sourceQuestionnaire;
            model.SourceQuestionnaireId = sourceQuestionnaire.QuestionnaireId;

            var availableQuestionnaireQuery = new QuestionnaireByOwnerAndStatus(User.Identity.Name, QuestionnaireStatus.Complete);
            var availableQuestionnaires = _repository.Find(availableQuestionnaireQuery).AsEnumerable();
            model.AvailableQuestionnaires = ToQuestionniareSelectList(availableQuestionnaires);

            return View(model);
        }

        private SelectList ToQuestionniareSelectList(IEnumerable<Questionnaire> availableQuestionnaires)
        {
            var items = availableQuestionnaires.Select(a =>
                new SelectListItem
                {
                    Value = a.QuestionnaireId.ToString(CultureInfo.InvariantCulture),
                    Text = a.QuestionniareName
                }).ToList();

            items.Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });

            return new SelectList(items, "Value", "Text");
        }

        [HttpPost]
        public ActionResult Copy(HomeCopyViewModel model)
        {
            var sourceQuestionniareQuery = new QuestionnaireById(model.SourceQuestionnaireId);
            var source = _repository.Find(sourceQuestionniareQuery);

            var targetQuestionniareQuery = new QuestionnaireById(model.TargetQuestionId);
            var target = _repository.Find(targetQuestionniareQuery);

            target.Status = QuestionnaireStatus.InProgress;

            while (target.Responses.Count > 0)
            {
                var response = target.Responses.FirstOrDefault();

                while (response != null && response.ResponseAudits.Count > 0)
                {
                    var responseAudit = response.ResponseAudits.FirstOrDefault();
                    _repository.Context.Remove(responseAudit);
                }
                _repository.Context.Remove(response);
            }

            _repository.Context.Update(target);
            _repository.Context.Commit();

            foreach (var response in source.Responses)
            {
                target.Responses.Add(new Response
                                     {
                                         Questionnaire = target,
                                         QuestionnaireId = target.QuestionnaireId,
                                         Comment = response.Comment,
                                         IsSelected = response.IsSelected,
                                         Value = response.Value,
                                         Created = DateTime.Now,
                                         CreatedBy = User.Identity.Name.ToUpper(),
                                         Modified = DateTime.Now,
                                         ModifiedBy = User.Identity.Name.ToUpper(),
                                         Question = response.Question,
                                         QuestionId = response.QuestionId
                                     });
            }

            _repository.Context.Update(target);
            _repository.Context.Commit();

            TempData["Message"] = "Results successfully copied!";

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            var model = new AboutViewModel
                        {
                            Name = "HR Survey",
                            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                            Description = "A Survey tool to allow Human Resource to solicit input about policies and procedures."
                        };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Reset(int id)
        {
            var query = new QuestionnaireById(id);
            var questionnaire = _repository.Find(query);

            questionnaire.Status = QuestionnaireStatus.NotStarted;
            questionnaire.CurrentCategoryId = null;
            questionnaire.Modified = DateTime.Now;
            questionnaire.ModifiedBy = User.Identity.Name;

            while (questionnaire.Responses.Count > 0)
            {
                var response = questionnaire.Responses.FirstOrDefault();

                while (response != null && response.ResponseAudits.Count > 0)
                {
                    var responseAudit = response.ResponseAudits.FirstOrDefault();
                    _repository.Context.Remove(responseAudit);
                }
                _repository.Context.Remove(response);
            }

            _repository.Context.Update(questionnaire);
            _repository.Context.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveCurrentCategory(string currentCategory, string questionnaireId, string surveyId)
        {
            try
            {
                var categoryId = int.Parse(currentCategory.Replace("CAT_", null));

                var questionnaireByIdQuery = new QuestionnaireById(Convert.ToInt32(questionnaireId));
                var questionnaire = _repository.Find(questionnaireByIdQuery);

                /* Update category id */
                questionnaire.CurrentCategoryId = categoryId;
                questionnaire.Modified = DateTime.Now;
                questionnaire.ModifiedBy = User.Identity.Name;

                _repository.Context.Update(questionnaire);
                _repository.Context.Commit();

                return Json("Success");
            }
            catch (DbEntityValidationException deve)
            {
                foreach (var eve in deve.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                      eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                          ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public JsonResult SetQuestionnaireStatus(JsonSetQuestionnaireStatus questionnaireStatus)
        {
            try
            {
                var query = new QuestionnaireById(questionnaireStatus.Id);
                var questionnaire = _repository.Find(query);
                questionnaire.Status = questionnaireStatus.Status;

                _repository.Context.Update(questionnaire);
                _repository.Context.Commit();

                return new JsonResult
                    {
                        Data = new { success = true },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = new { success = false, error = "Unable to set questionnaire to complete..." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public JsonResult DisplayLogicDisable(JsonDisplayLogicDisable displayLogicDisable)
        {
            try
            {
                var query = new QuestionnaireById(displayLogicDisable.QuestionnaireId);
                var questionnaire = _repository.Find(query);

                var responses = questionnaire.Responses.Where(x => x.QuestionnaireId == displayLogicDisable.QuestionnaireId && x.QuestionId == displayLogicDisable.QuestionId).ToList();

                foreach (var response in responses)
                {
                    _repository.Context.Remove(response);
                }
                _repository.Context.Commit();

                var disabledResponse = new Response
                    {
                        Value = "_DISABLED_",
                        IsSelected = "True",
                        QuestionnaireId = questionnaire.QuestionnaireId,
                        QuestionId = displayLogicDisable.QuestionId,
                        Created = DateTime.Now,
                        CreatedBy = User.Identity.Name.ToUpper(),
                        Modified = DateTime.Now,
                        ModifiedBy = User.Identity.Name.ToUpper()
                    };

                questionnaire.Responses.Add(disabledResponse);

                _repository.Context.Update(questionnaire);
                _repository.Context.Commit();

                return new JsonResult
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = new { success = false, error = "Unable to save disabled response..." },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public JsonResult SaveQuestionResponses(JsonQuestionResponse question)
        {
            var query = new QuestionnaireById(question.QuestionnaireId);
            var questionnaire = _repository.Find(query);

            var questionQuery = new QuestionById(question.QuestionId);
            var questionResult = _repository.Find(questionQuery);

            foreach (var item in question.Responses)
            {
                Response response;

                if (questionResult.IsTextBased)
                {
                    response = questionnaire.Responses
                        .SingleOrDefault(x => x.QuestionId == item.QuestionId && x.QuestionnaireId == item.QuestionnaireId);
                }
                else
                {
                    response = questionnaire.Responses
                        .SingleOrDefault(x => x.QuestionId == item.QuestionId && x.QuestionnaireId == item.QuestionnaireId && x.Value == item.Value);
                }

                if (response == null)
                {
                    response = new Response
                    {
                        QuestionnaireId = question.QuestionnaireId,
                        QuestionId = question.QuestionId,
                        Comment = item.Comment,
                        Created = DateTime.Now,
                        CreatedBy = User.Identity.Name.ToUpper(),
                        ModifiedBy = User.Identity.Name.ToUpper(),
                        Modified = DateTime.Now,
                        Value = item.Value,
                        IsSelected = item.IsSelected
                    };

                    _repository.Context.Add(response);
                }
                else
                {
                    var originalIsSelected = response.IsSelected;
                    var originalValue = response.Value;

                    response.ModifiedBy = User.Identity.Name;
                    response.Modified = DateTime.Now;
                    response.Value = item.Value;
                    response.IsSelected = item.IsSelected;
                    response.Comment = item.Comment;

                    _repository.Context.Update(response);

                    if (originalIsSelected != item.IsSelected || originalValue != item.Value)
                    {
                        var responseAudit = new ResponseAudit
                                            {
                                                ResponseId = response.ResponseId,
                                                Response = response,
                                                OldIsSelectedValue = originalIsSelected,
                                                NewIsSelectedValue = item.IsSelected,
                                                OldResponseValue = originalValue,
                                                NewResponseValue = item.Value,
                                                Created = DateTime.Now,
                                                CreatedBy = User.Identity.Name.ToUpper()
                                            };
                        _repository.Context.Add(responseAudit);
                    }
                }

                _repository.Context.Commit();

            }

            return Json("Success");
        }

        public JsonResult SaveQuestionResponse(JsonResponse response)
        {
            var query = new QuestionnaireById(response.QuestionnaireId);
            var questionnaire = _repository.Find(query);

            var questionQuery = new QuestionById(response.QuestionId);
            var questionResult = _repository.Find(questionQuery);

            Response item;

            if (questionResult.IsTextBased)
            {
                item = questionnaire.Responses
                                    .SingleOrDefault(
                                        x =>
                                        x.QuestionId == response.QuestionId &&
                                        x.QuestionnaireId == response.QuestionnaireId);
            }
            else
            {
                item = questionnaire.Responses
                                    .SingleOrDefault(
                                        x =>
                                        x.QuestionId == response.QuestionId &&
                                        x.QuestionnaireId == response.QuestionnaireId && x.Value == response.Value);
            }

            if (item == null)
            {
                item = new Response {Created = DateTime.Now, CreatedBy = User.Identity.Name.ToUpper()};
            }

            item.QuestionnaireId = response.QuestionnaireId;
            item.QuestionId = response.QuestionId;
            item.Comment = response.Comment;

            item.ModifiedBy = User.Identity.Name.ToUpper();
            item.Modified = DateTime.Now;
            item.Value = response.Value;
            item.IsSelected = response.IsSelected;

            if (item.ResponseId == 0)
                _repository.Context.Add(item);
            else
                _repository.Context.Update(item);

            _repository.Context.Commit();


            return Json("Success");
        }
    }
}