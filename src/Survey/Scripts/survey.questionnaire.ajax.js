/// <reference path="jquery-1.8.2.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="survey.questionniare.general.js" />
/// <reference path="survey.questionniare.displaylogic.js" />

function SetQuestionnareStatus(questionnaireId, status, uri) {
    var statusUpdate = {};
    statusUpdate.Id = questionnaireId;
    statusUpdate.status = status;

    $.ajax({
        url: uri,
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(statusUpdate),
        contentType: 'application/json: charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(xhr.responseText);
            alert(thrownError);
        },
        async: false
    });
}

//function ajaxDisplayLogicDisabled(questionniareId, questionId, uri) {
//    displayLogicDisable = { QuestionnaireId: questionnaireId, QuestionId: questionId };
//    var data = JSON.stringify({ displayLogicDisable: displayLogicDisable });
//    $.ajax(
//        {
//            type: "POST",
//            url: uri,
//            dataType: "json",
//            contentType: "application/json; charset=utf-8",
//            data: data,
//            success: function () { /* Success; do nothing */
//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                alert(xhr.status);
//                alert(xhr.responseText);
//                alert(thrownError);
//            },
//            async: false
//        });
//}

function SaveCurrentCategory(questionniareId, uri) {
    var questionnaireId = questionniareId;
    var currentCategoryCode = $("#Code").val();
    var surveyId = $("#" + currentCategoryCode).attr("surveyId");

    $.ajax(
        {
            type: "POST",
            url: uri,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ currentCategory: currentCategoryCode, questionnaireId: questionnaireId, surveyId: surveyId }),
            success: function () {
                window.ShowCurrentCategory();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(xhr.responseText);
                alert(thrownError);
            },
            async: false
        });
}

function SaveQuestionResponses(questionnaireId, questionId, responses, uri) {
    /* Create an object */
    question = { QuestionnaireId: questionnaireId, QuestionId: questionId, Responses: responses };

    /* Saving previous question's response to db */
    $.ajax(
        {
            type: "POST",
            url: uri,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ question: question }),
            success: function () { /* Success; do nothing */
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(xhr.responseText);
                alert(thrownError);
            },
            async: false
        });
}

function BuildResponseObject(previousOrCurrentCategory) {

    var questionResponseArray = new Array();
    var selectedCategory;

    if (previousOrCurrentCategory == "Previous") {
        selectedCategory = window.previousCategory;
    } else {
        selectedCategory = $("#Code option:selected").val();
    }

    var rows = $("#" + selectedCategory).find("tr");
    for (var r = 0; r < rows.length; r++) {
        var row = $(rows[r]);
        var questionId = row.attr("questionId");
        
        //TODO: Fix for display logic, saved questions are adding records should set all responses to false and add a response for disabled.
        var disabled = row.attr("disabled");
        var isDisabled = row.attr("disabled") == "disabled";

        var cells = $(row).find("td");
        for (var c = 0; c < cells.length; c++) {
            var cell = cells[c];

            if ($(cell).find("option").length) {

                var values = $(cell).find("option[value]");
                for (var v = 0; v < values.length; v++) {
                    var value = values[v];

                    if ($(value).text() != "") {
                        questionResponseArray.push(SetResponseProperties(value, questionId, $(value).text(), "selected"));
                    }
                }
            } else if ($(cell).find("input").length) {
                var inputs = $(cell).find("input");

                for (var i = 0; i < inputs.length; i++) {
                    var input = inputs[i];
                    var isCommentBox = false;

                    var attribute;
                    var actionType;
                    /* Get the input where type equals to "text" and id doesn't contain "Comment" */
                    if ($(input).attr("type") == "text" && $(input).attr("id").indexOf("Comment") == -1) {
                        attribute = "value";
                        actionType = "answered";
                    } else if ($(input).attr("type") == "radio" || $(input).attr("type") == "checkbox") {
                        attribute = "displayText";
                        actionType = "checked";
                    } else {
                        isCommentBox = true; /* Skip the input where id contains "Comment" */
                    }
                    if (isCommentBox == false) {
                        questionResponseArray.push(SetResponseProperties(input, questionId, $(input).attr(attribute), actionType));
                    }
                }
            }
        }
    }
    return questionResponseArray;
}

function SetResponseProperties(currentObject, questionId, value, actionType) {
    var questionResponse = new Object();
    questionResponse.Value = value;

    /* For text type input, we will only use "actionType" to do the validation */
    if (($(currentObject).attr(actionType) == "checked" || actionType == "answered"
        || $(currentObject).attr(actionType) == "selected") && currentObject.isDisabled == false) {
        questionResponse.IsSelected = "True";
    } else {
        if (currentObject.type == "text" && questionResponse.Value != "")   /* Remove text value */ {
            questionResponse.Value = "";
        }
        questionResponse.IsSelected = "False";
    }

    if ((value.indexOf("Other") != -1) && questionResponse.IsSelected == "True") {
        var objectName;

        if (actionType == "selected") {
            objectName = $(currentObject).parent().attr("name");
        } else {
            objectName = $(currentObject).attr("name");
        }

        questionResponse.Comment = $("#" + objectName + "_Comment").val();
    } else {
        questionResponse.Comment = "";
    }

    questionResponse.QuestionId = questionId;
    questionResponse.QuestionnaireId = QuestionnaireId();

    return questionResponse;
}