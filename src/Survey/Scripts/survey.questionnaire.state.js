/// <reference path="jquery-1.8.2.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />

function QuestionnaireState() {

    var required = new Array();
    var answered = new Array();
    var unAnswered = new Array();

    this.addRequiredIds = addRequiredIds;
    function addRequiredIds(itemArray) {
        if (itemArray instanceof Array) {
            for (var i = 0; i < itemArray.length; i++) {
                var item = itemArray[i];
                addRequiredId(item);
            }
        } else {
            alert("invalid parameter");
        }
    }

    this.addRequiredId = addRequiredId;
    function addRequiredId(item) {
        if (!requiredContains(item)) {
            required.push(item);
        }
    }

    this.removeRequiredId = removeRequiredId;
    function removeRequiredId(item) {
        for (var i = 0; i < required.length; i++) {
            if (required[i] == item) {
                required.splice(i, 1);
            }
        }
    }

    this.requiredContains = requiredContains;
    function requiredContains(item) {
        return $.inArray(item, required) !== -1;
    }

    this.addAnsweredIds = addAnsweredIds;
    function addAnsweredIds(itemArray) {
        if (itemArray instanceof Array) {
            for (var i = 0; i < itemArray.length; i++) {
                var item = itemArray[i];
                addAnsweredId(item);
            }
        } else {
            alert("invalid parameter");
        }
    }

    this.addAnsweredId = addAnsweredId;
    function addAnsweredId(item) {
        if (!answeredContains(item)) {
            answered.push(item);
        }
    }

    this.removeAnsweredId = removeAnsweredId;
    function removeAnsweredId(item) {
        for (var i = 0; i < answered.length; i++) {
            if (answered[i] == item) {
                answered.splice(i, 1);
            }
        }
    }
    
    this.answeredContains = answeredContains;
    function answeredContains(item) {
        return $.inArray(item, answered) !== -1;
    }

    this.removeFromRequiredAndAnswered = removeFromRequiredAndAnswered;
    function removeFromRequiredAndAnswered(item) {
        removeRequiredId(item);
        removeAnsweredId(item);
    }

    this.isComplete = isComplete;
    function isComplete() {
        var complete = true;
        unAnswered = new Array();

        for (var i = 0; i < required.length; i++) {
            var id = required[i];

            var noMatchFound = $.inArray(id, answered);
            if (noMatchFound === -1) {
                complete = false;
                unAnswered.push(id);
            }
        }
        return complete;
    }

    this.getUnanswered = getUnanswered;
    function getUnanswered() {
        return unAnswered;
    }
}