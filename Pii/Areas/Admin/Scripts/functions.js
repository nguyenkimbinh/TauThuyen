function SubmitForm(idbutton, idform) {
    $(idbutton).click(function () {
        $(idform).submit();
    });
}
function BrowseImages(inputId) {
    var finder = new CKFinder();
    finder.BasePath = ckfinder;
    finder.SelectFunction = SetImageField;
    finder.SelectFunctionData = inputId;
    finder.Popup();
}
function SetImageField(fileUrl, data) {
    var id = "#" + data["selectActionData"];
    $(id).parent().find("img").attr({ "src": fileUrl});
    $(id).val(fileUrl);
}
function SetNull(idInput) {
    idInput = "#" + idInput;
    $(idInput).val(null);
    return false;
}
// ImagesAttribute:
function ImagesAttribute() {
    var template = $("#add_image_template");
    $(template).appendTo("body");
    $(template).hide();
}
ImagesAttribute();

$(document).ready(function () {
    $("input[type='text']#Title").blur(function () {
        $.post(MakeAlias, {
            title: $(this).val()
        }, function (data) {
            if (data) {
                $("#Alias").val(data);
            }
        });
    });
    $(".images_attribute a.add").click(function () {
        var id = $(this).parent().find("input").attr("id");
        BrowseImages(id);
    });
    $(".images_attribute a.delete").click(function () {
        var inputs = $(this).parent().find("input.image_link");
        $(inputs).val(null);
        return false;
    });
});