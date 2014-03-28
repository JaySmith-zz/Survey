/// <reference path="jquery-1.8.2.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="survey.questionnaire.state.js" />
/// <reference path="survey.questionnaire.general.js" />
/// <reference path="survey.questionnaire.ajax.js" />

function ApplyDisplayLogic(input, maxDepth, currentDepth, result) {

    if (currentDepth > maxDepth) return;

    var questionCode = GetQuestionCode(input);
    var questionValue = window.GetSelectedValueForInput(input);

    var affectedQuestions = FindAffectedQuestions(input, questionCode, questionValue);
    for (var i = 0; i < affectedQuestions.length; i++) {
        var item = affectedQuestions[i];
        var questionId = item.getAttribute("questionId");
        var displayLogic = item.getAttribute("displayLogic");

        if (currentDepth == 0) result = EvaluateDisplayLogic(displayLogic, questionCode, questionValue);
        
        if (result) {
            item.setAttribute("disabled", "disabled");
            state.removeFromRequiredAndAnswered(questionId);
            ClearInputValue(questionId);
            DisableInputsForQuestion(questionId);
        } else {
            item.removeAttribute("disabled");
            EnableInputsForQuestion(questionId);
        }

        if (displayLogic.indexOf("==") != -1) {
            ApplyDisplayLogic(item, maxDepth, ++currentDepth, result);
        }
    }
}

function EvaluateDisplayLogic(expression, token, value) {

    var isInverseExpression = expression.indexOf("!=") != -1;

    expression = expression.replace("!=", "==");
    expression = expression.replace(token, '"' + value + '"');

    var result = eval(expression);

    if (isInverseExpression) {
        return !result;
    } else {
        return result;
    }
}

function ClearInputValue(questionId) {

    var questions = $("input[name=" + questionId + "]");

    /* If inputs.length equals to 0, we need to check to see if input is a drop down list. */
    if (questions.length == 0) {
        questions = $("select[name=" + questionId + "]");
    }

    for (var i = 0; i < questions.length; i++) {
        var question = questions[i];

        var itemType = $(question).prop("type");

        switch (itemType.toLowerCase()) {
            case "checkbox":
            case "radio":
                $(question).filter(":checked").removeAttr("checked");
                HandleOtherCommentForCheckboxAndRadioButton(question);
                break;
            case "select-one":
                $(question).val([]);
                HandleOtherCommentForDropdown(question);
                break;
            case "text":
                $(question).val("");
                break;
            default:
                break;
        }
    }
}

function DisableInputsForQuestion(questionId) {
    var questions = $("input[name=" + questionId + "]");

    /* If inputs.length equals to 0, we need to check to see if input is a drop down list. */
    if (questions.length == 0) {
        questions = $("select[name=" + questionId + "]");
    }

    for (var i = 0; i < questions.length; i++) {
        var question = questions[i];
        $(question).attr("disabled", "disabled");
    }
}

function EnableInputsForQuestion(questionId) {
    var questions = $("input[name=" + questionId + "]");

    /* If inputs.length equals to 0, we need to check to see if input is a drop down list. */
    if (questions.length == 0) {
        questions = $("select[name=" + questionId + "]");
    }

    for (var i = 0; i < questions.length; i++) {
        var question = questions[i];
        $(question).removeAttr("disabled");
    }
}

function FindAffectedQuestions(input, questionCode, value) {

    if (input.type == "checkbox") {
        return FindAffectedQuestionsForCheckbox(questionCode, value);
    }
    return FindAffectedQuestionsDefault(questionCode);
}

function FindAffectedQuestionsDefault(questionCode) {

    var rows = [];
    var possibleRows = $('tr[displayLogic~="' + questionCode + '"]');

    for (var i = 0; i < possibleRows.length; i++) {

        var item = possibleRows[i];
        var displayLogic = item.getAttribute("displayLogic");
        var code = displayLogic.split(" ")[0].replace(/^\s+|\s+$/g, "");

        if (questionCode == code) {
            rows.push(item);
        }
    }

    return rows;
}

function FindAffectedQuestionsForCheckbox(questionCode, value) {

    var rows = [];
    var possibleRows = $('tr[displayLogic~="' + questionCode + '"]');

    for (var i = 0; i < possibleRows.length; i++) {

        var item = possibleRows[i];
        var displayLogic = item.getAttribute("displayLogic");
        var containsValue = displayLogic.indexOf(value) != -1;

        if (containsValue) {
            rows.push(item);
        }
    }

    return rows;
}