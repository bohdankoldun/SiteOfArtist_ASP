/* automation resize textArea*/

function resizeTextarea(id) {
    var a = document.getElementById(id);
    a.style.height = 'auto';
    a.style.height = a.scrollHeight + 'px';

    var counter = "Введено " + $(a).val().length + " символів";
    $('span[data-valmsg-for="biography_val"]').text(counter);
}

/* -------------------------------------------------------------------------*/


/* adding and showing photos*/

function showFile(e) {
    var files = e.target.files;
    var listImages = document.getElementById('images-line')

    //add new input for image
    var oldInput = e.target;
    var newInput = oldInput.cloneNode(true);

    //add new input and add listener to it
    document.getElementById('input_files').insertBefore(newInput, null);
    newInput.addEventListener('change', showFile, false);

    //hide old input
    e.target.style.display = 'none';

    //add to ol a new photo
    for (var i = 0, f; f = files[i]; i++) {
        if (!f.type.match('image.*')) continue;
        var fr = new FileReader();
        fr.onload = (function (theFile) {
            return function (e) {
                var image = document.createElement('img');
                image.src = e.target.result;
                image.style.border = "3px solid green";
                listImages.insertBefore(image, null);
            };
        })(f);

        fr.readAsDataURL(f);
    }

    //hide warning
    $('span[data-valmsg-for="Photos"]').text('');
}

/* -------------------------------------------------------------------------*/

/*for delete photos*/
function deletePhoto(e) {
    var photo = e.target;
    if (photo.style.border != "3px solid red") {
        photo.style.border = "3px solid red";
        $("#form-editing-artist").append('<input id="url" name="url" type="hidden" value="' + photo.getAttribute('src') + '">');
    }
    else {
        photo.style.border = "";
        $("input[value='" + photo.getAttribute('src') + "']").remove();
    }

}



/*for adding and deleting inputs*/
function addField(e) {
    var btn = e.target;
    var id = btn.id;
    $('<input class="form-control text-box single-line" name="' + id + '" type="text" value="" />').insertBefore("#" + id);


}

function deleteChange(e) {
    var input = e.target;
    var text = input.value;
    if (text == "")
        input.remove();


}
/* -------------------------------------------------------------------------*/


function init(e) {
    //init resize textarea
    var a = document.getElementsByTagName('textarea');
    for (var i = 0, inb = a.length; i < inb; i++)
        resizeTextarea(a[i].id);

    //init button to add contact
    var buttons = $(".btn-add-contact-item");

    for (var i = 0; i < buttons.length; i++)
        buttons[i].addEventListener("click", addField);

    var inputs = $('input[type="text"]');

    for (var i = 0; i < inputs.length; i++)
        inputs[i].addEventListener("change", deleteChange);

    //add listner to first input photo element
    var input_file = document.getElementsByClassName('files')[0];
    input_file.addEventListener('change', showFile, false);

    //initDeletePhoto(e) 
    var photos = $("img");

    for (var i = 0; i < photos.length; i++)
        photos[i].addEventListener("click", deletePhoto);

}

addEventListener('DOMContentLoaded', init);

