var optionCounter = 0;
var removeButtonsCounter = 0;
var surveyItemsCounter = 0;
//var i = 0;
$(document).ready(function () {

    $(document).on("click", ".ss-ghost", function () {
        var selectedOption = $("#dropDown option:selected").text();
        if (selectedOption === "Multiple Choice") {
            var nextMc = $("#multipleChoiceItem").clone();
            nextMc.attr("id", "multipleChoiceItem" + ++optionCounter);
            nextMc.children("div").children("span").attr("id", "removeButton" + ++removeButtonsCounter);
            var len = $(".parentOfItems").children().length;
            $(".parentOfItems").children().eq(len - 1).after(nextMc);                       
            $("#removeButton" + removeButtonsCounter).show();
        }      
    });

    
});

$(document).on("change", ".ss-q-radio", function () {
    if ($(this).prop("checked")) {
        var choice = $(this).attr("value");
        $(this).parent().parent().parent().parent().children().eq(0).val(choice);
    }
});

$(document).on("change", "#dropDown", function () {
    var selectedOption = $("#dropDown option:selected").text();

    $("#text1").hide();                     //TEXT
    $("#text2").hide();                     //PARAGRAPH TEXT
    $("#choice1").hide();                   //MULTIPLE CHOICE

    if (selectedOption === "Multiple Choice") {
        $("#choice1").show();
    }
    else if (selectedOption === "Text") {
        $("#text1").show();      
    }
    else if (selectedOption === "Paragraph Text") {
        $("#text2").show();      
    }
});

function removeItem(e) {
    $(e).parent("div").parent("div").remove();
 //   i--;
}
function removeRow(e) {
    $(e).parent("div").parent("div").remove();
}

function addItem() { 
    var question = $("#questionText").val();
    var selectedOption = $("#dropDown option:selected").text();
    if (selectedOption === "Text") {
        var newTextObject = $("#answerType1").clone();
        newTextObject.attr("id", ++surveyItemsCounter);
        newTextObject.children().eq(1).children(".ss-q-title").attr("value", question);
        newTextObject.children().eq(1).children(".ss-q-title").text(/*(++i).toString()+ " " + */question);
        var childrenNum = $("#surveyQuestions").children().length;
        $("#surveyQuestions").children().eq(childrenNum - 1).after(newTextObject);
        newTextObject.show();
    } else if (selectedOption === "Paragraph Text") {
        var newParagraphObject = $("#answerType2").clone();
        newParagraphObject.attr("id", ++surveyItemsCounter);
        newParagraphObject.children().eq(1).children(".ss-q-title").attr("value", question);
        newParagraphObject.children().eq(1).children(".ss-q-title").text(/*(++i).toString()+ " " + */question);
        var childrenNum2 = $("#surveyQuestions").children().length;
        $("#surveyQuestions").children().eq(childrenNum2 - 1).after(newParagraphObject);
        newParagraphObject.show();
    } else if (selectedOption === "Multiple Choice") {
        //Clone the answer type
        var newMultipleObject = $("#answerType3").clone();
        var optionNum = 0;
        $("[name=multiple1]").each(function () {
            optionNum++;
            //Get creator's choices (working good)
            var text = $(this).children().eq(0).children().eq(3).val();
            //Clone a button to put it in the survey
            var radioButton = $("#answerType3").children().eq(1).children().eq(3).children().eq(0).clone();
            //overwrite its original text with 'text'
            radioButton.children().eq(0).children(".ss-choice-label").text(text);
            radioButton.children().eq(0).children(".ss-choice-label").attr("value", text);
            //get the parent where current option will be added to
            var parent = newMultipleObject.children().eq(1);
            //get child num (3)
            var childrenNum3 = parent.children().length;
            //put the currentOption as the last child
            parent.children().eq(childrenNum3 - 1).after(radioButton);
        });
        //delete the original button
        newMultipleObject.children().eq(1).children().eq(3).children().eq(0).remove();
        
        //Change question title and add it to the survey
        newMultipleObject.attr("id", ++surveyItemsCounter);
        newMultipleObject.children().eq(1).children(".option-num").attr("value", optionNum);
        newMultipleObject.children().eq(1).children(".ss-q-title").attr("value", question);
        newMultipleObject.children().eq(1).children(".ss-q-title").text(/*(++i).toString()+ " " + */question);
        var childrenNum3 = $("#surveyQuestions").children().length;
        $("#surveyQuestions").children().eq(childrenNum3 - 1).after(newMultipleObject);
        newMultipleObject.show();
    }
}