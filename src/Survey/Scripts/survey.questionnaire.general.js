/// <reference path="jquery-1.8.2.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="survey.questionnaire.state.js" />

var questionnaireId = -1;
var questionnaireId = $("#hidQuestionnaireId").val();
var surveyId = $("#hidSurveyId").val();

function CategoryDropDownListChange() {
    window.SaveData("Previous");
    window.ShowCurrentCategory();
    window.SaveCurrentCategory(questionnaireId, window.ajaxSaveCurrentCategoryUrl);
}

function ShowCurrentCategory() {

    var selectCategory = $("#Code").get(0);
    var selected = selectCategory[selectCategory.selectedIndex];
    var selectedValue = selected.value;
    var selectLength = selectCategory.options.length;

    if (selectCategory.selectedIndex == 0) {
        window.ButtonPreviousDisable();
        window.ButtonNextEnable();
    } else if (selectCategory.selectedIndex + 1 == selectLength) {
        window.ButtonPreviousEnable();
        window.ButtonNextDisable();
    } else {
        window.ButtonPreviousEnable();
        window.ButtonNextEnable();
    }

    // hide all categories
    var divs = $("div[id]:visible");
    for (var i = 0; i < divs.length; i++) {
        if (divs[i].id != "body") {
            $(divs[i]).hide();
        }
    }

    // Show current category
    $("div[id='" + selectedValue + "']").show();

    ValidateCommentBox();
}

function ButtonNextClick() {
    window.SaveData("Current");

    var selectedItem = $("#Code option:selected").index();
    if (selectedItem == -1) selectedItem = 0;

    //selectedItem++; /* Move to next category */
    selectedItem = FindNextCategoryWithActiveQuestions();

    var itemFilter = '#Code option:eq(' + selectedItem + ')';
    $(itemFilter).prop('selected', 'true'); /* set ddl value */

    window.SaveCurrentCategory(questionnaireId, window.ajaxSaveCurrentCategoryUrl);
}

function ButtonPreviousClick() {
    window.SaveData("Current");

    var selectedItem = $("#Code option:selected").index();
    if (selectedItem == -1) selectedItem = 0;

    //selectedItem--; /* Move to previous category */
    selectedItem = FindPreviousCategoryWithActiveQuestions();
    var itemFilter = '#Code option:eq(' + selectedItem + ')';
    $(itemFilter).prop('selected', 'true'); /* set ddl value */
    window.SaveCurrentCategory(questionnaireId, window.ajaxSaveCurrentCategoryUrl);
}

function FindNextCategoryWithActiveQuestions() {
    var options = $("#Code option");
    var selectedIndex = $("#Code option:selected").index();
    selectedIndex++;    // move to the next category

    for (var i = selectedIndex; i < options.length; i++) {
        var option = options[i];
        var id = option.value;

        var inputCount = $("#" + id + " table tr").length;
        var disabledCount = $("#" + id + " table tr[disabled='disabled']").length;
        
        if (disabledCount < inputCount) {
            selectedIndex = i;
            break;
        }
    }

    return selectedIndex;
}

function FindPreviousCategoryWithActiveQuestions() {
    var options = $("#Code option");
    var selectedIndex = $("#Code option:selected").index();
    selectedIndex--;    // move to the next category

    for (var i = selectedIndex; i < selectedIndex+1; i--) {
        var option = options[i];
        var id = option.value;

        var inputCount = $("#" + id + " table tr").length;
        var disabledCount = $("#" + id + " table tr[disabled='disabled']").length;

        if (disabledCount < inputCount) {
            selectedIndex = i;
            break;
        }
    }

    return selectedIndex;
}

function TextboxChangeHandler() {
    switch (this.id) {
        case "number":
            if (this.value != "" && !isNaN(this.value)) {
                if (state.answeredContains(this.name) == false) {
                    state.addAnsweredId(this.name);
                }
            } else {
                state.removeAnsweredId(this.name);
            }
            break;
        default:
            if (this.value != "") {
                if (state.answeredContains(this.name) == false) {
                    state.addAnsweredId(this.name);
                }
            } else if (state.answeredContains(this.name)) {
                state.removeAnsweredId(this.name);
            }
            break;
    }

    window.ApplyDisplayLogic(this, window.maxDisplayLogicDepth, 0);
    window.EnableCompleteButton();
}

function ValidateCommentBox() {
    for (var i = 0; i < window.requiredQuestionIds.length; i++) {
        if (window.requiredQuestionIds[i].indexOf("_Comment") != -1) {
            var selector = "#" + window.requiredQuestionIds[i];
            ValidateSingleElement(selector);
        }
    }
}

function ValidateSingleElement(selector) {
    window.questionnaireForm.validate().element(selector);
}

function QuestionnaireId() {
    return questionnaireId;
}

function GetQuestionCode(item) {
    return item.getAttribute("questionCode");
}

function ShowOther(selector) {
    state.addRequiredId(selector);

    if (selector.indexOf("#") == -1) {
        selector = "#" + selector;
    }

    $(selector).show();
    ValidateSingleElement(selector);
}

function HideOther(selector) {
    state.removeRequiredId(selector);

    if (selector.indexOf("#") == -1) {
        selector = "#" + selector;
    }

    $(selector).val("");
    $(selector).hide();
    HideRedAsterisk(selector);
}

function HideRedAsterisk(selector) {
    var elementName = selector.replace("#", '');
    $("span[data-valmsg-for='" + elementName + "']").html("");
}

function HandleOtherCommentForCheckboxAndRadioButton(input) {
    var value = input.getAttribute("displayText").toUpperCase();
    var inputName = input.getAttribute("name");
    var elementName = inputName + "_Comment";
    var selector = "#" + elementName;

    if (/OTHER/i.test(value) && input.checked) {
        state.addRequiredId(elementName);
        if ($(selector).val().length > 0) {
            state.AnsweredQuestionsAdd(elementName);
        }
        ShowOther(elementName);
    } else {
        var isOtherChecked = $("input[type=checkbox][name='" + inputName + "'][value=Other]").is(":checked");
        if (isOtherChecked == false) {
            HideOther(elementName);
        }
    }
}

function HandleOtherCommentForDropdown(input) {
    var id = input.id;
    var selectedText = $("select[id='" + id + "']").children("option").filter(":selected").text();
    var elementName = id + "_Comment";
    var selector = "#" + elementName;

    if (selectedText.indexOf("Other") != -1) {
        state.addRequiredId(elementName);
        if ($(selector).val().length > 0) {
            state.addAnsweredId(elementName);
        } else {
            state.removeAnsweredId(elementName);
        }
        ShowOther(elementName);
    } else {
        state.removeAnsweredId(elementName);
        HideOther(elementName);
    }
}

function HightlightUnanswered() {

    $("table").find("TR:odd").css("background-color", "#FFFFFF");
    $("table").find("TR:even").css("background-color", "#EEEFF6");

    var unanswered = state.getUnanswered();
    for (var i = 0; i < unanswered.length; i++) {
        var questionId = unanswered[i];
        var selector = "tr[questionId=" + questionId + "]";
        $(selector).css("background-color", "#ffff00");
    }
}

function SaveResponsesToQuestionForCategory(category) {

    var tableRows = $("#" + category + " table tr");

    for (var i = 0; i < tableRows.length; i++) {
        var row = $(tableRows[i]);
        SaveResponseToQuestion(row);
    }
}

function SaveResponseToQuestion(row) {

    var questionnaireId = window.QuestionnaireId();
    var questionId = row.attr("questionId");

    var inputsInRow = row.find(":input");

    var responses = GetResponseObjectsForInputs(inputsInRow, questionId);

    window.SaveQuestionResponses(questionnaireId, questionId, responses, window.ajaxSaveQuestionResponsesUrl);
}

function GetResponseObjectsForInputs(inputs, questionId) {

    var inputType = inputs[0].type;

    var responses;
    if (inputType == "select-one") {
        responses = GetResponseObjectsForDropdown(inputs, questionId);
    } else {
        responses = GetResponsObjectsForInputsDefault(inputs, questionId);
    }

    return responses;
}

function GetResponseObjectsForDropdown(inputs, questionId) {

    var responses = new Array();
    for (var j = 0; j < inputs[0].options.length; j++) {
        var option = inputs[0].options[j];

        if (option.value != "") {
            var response = GetResponseForDropdown(option);
            response.QuestionnaireId = window.QuestionnaireId();
            response.QuestionId = questionId;
            response.Comment = GetCommentForInput(response.Value, questionId);

            responses.push(response);
        }
    }

    return responses;
}

function GetResponsObjectsForInputsDefault(inputs, questionId) {

    var responses = new Array();
    for (var i = 0; i < inputs.length; i++) {
        var input = $(inputs[i]);

        var response = GetResponseObjectForInput(input);
        response.QuestionnaireId = window.QuestionnaireId();
        response.QuestionId = questionId;
        response.Comment = GetCommentForInput(response.Value, questionId);

        responses.push(response);
    }

    return responses;
}


function GetResponseObjectForInput(input) {

    var response = new Object();
    switch (input.context.type) {
        case "radio":
        case "checkbox":
            response = GetResponseForRadioButton(input);
            break;
        case "text":
            response.IsSelected = true;
            response.Value = $(input).val();
            break;
    }

    return response;
}

function GetResponseForRadioButton(input) {
    var response = new Object();

    var isDisabled = IsInputDisabled(input);
    var isSelected = input.attr("checked") == "checked";

    if (isDisabled) {
        response.IsSelected = "_NA_";
    } else {
        response.IsSelected = isSelected;
    }

    response.Value = GetValueForInput(input);

    return response;
}

function GetResponseForDropdown(input) {

    var isDisabled = input.isDisabled;

    var response = new Object();
    response.IsSelected = input.selected;
    response.Value = input.text;

    if (isDisabled) {
        response.IsSelected = "_NA_";
    }

    return response;
}

function IsInputDisabled(input) {
    var parentRow = $(input).closest('tr');
    return parentRow.attr("disabled") == 'disabled';
}


function GetValueForInput(input) {

    var value;

    switch (input.attr("type")) {
        case "radio":
        case "checkbox":
            value = input.attr("displayText");
            break;
        default:
            value = input.value;
            break;
    }

    return value;
}

function GetSelectedValueForInput(input) {

    var value;

    switch (input.type) {
        case "radio":
            value = GetSelectedValueForRadioButton(input);
            break;
        case "select-one":
            value = GetSelectedValueForDropdown(input);
            break;
        default:
            value = input.value;
            break;
    }

    return value;
}

function GetSelectedValueForRadioButton(input) {
    var value = "";

    var selected = $("input[type='radio'][name='" + input.name + "']:checked");
    if (selected.length > 0) {
        value = selected.attr("displayText");
    }

    return value;
}

function GetSelectedValueForDropdown(input) {
    return $(input).children("option").filter(":selected").text();
}

function GetCommentForInput(text, questionId) {

    var value = "";
    if (text.indexOf("Other") != -1) {
        value = $("#" + questionId + "_Comment").val();
    }

    return value;
}


// need to disable the row 
//var parentRow = $(this).closest('tr');
// Move this to category change handler
//if (inputsDisabled.length < inputsEnabled.length) {