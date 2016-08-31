/* automation resize textArea*/

function resizeTextarea(id) {
    var a = document.getElementById(id);
    a.style.height = 'auto';
    a.style.height = a.scrollHeight + 'px';

    a.style.backgroundImage = "linear-gradient(rgb(15, 238, 60), rgb(130, 255, 122))";

    var counter = "Введено " + $(a).val().length + " символів";
    $('span[data-valmsg-for="Comment"]').text(counter);
}

function resize(e) {
    var a = e.target;
    a.style.height = 'auto';
    a.style.height = a.scrollHeight + 'px';
}

function changeBackground(e) {
    e.target.style.backgroundImage = "linear-gradient(rgb(15, 238, 60), rgb(130, 255, 122))";
}


function init() {
    var a = document.getElementsByTagName('textarea');
    var b = document.getElementsByTagName('input');

    for (var i = 0, inb = a.length; i < inb; i++)
        a[i].addEventListener('focus', resize);


    for (var i = 0, inb = b.length; i < inb; i++)
        b[i].addEventListener('change', changeBackground);
}

addEventListener('DOMContentLoaded', init);
/* -------------------------------------------------------------------------*/


/* adding and showing photos*/

function showFile(e) {
    var files = e.target.files;
    var listImages = document.getElementById('list_upload_image')

    //add new input for image
    var oldInput = e.target;
    var newInput = oldInput.cloneNode(true);

    //add new input and add listener to it
    document.getElementById('input_files').insertBefore(newInput, listImages);
    newInput.addEventListener('change', showFile, false);

    //hide old input
    e.target.style.display = 'none';

    //add to ol a new photo
    for (var i = 0, f; f = files[i]; i++) {
        if (!f.type.match('image.*')) continue;
        var fr = new FileReader();
        fr.onload = (function (theFile) {
            return function (e) {
                var li = document.createElement('li');
                li.innerHTML = "<img src='" + e.target.result + "' />";
                listImages.insertBefore(li, null);
            };
        })(f);

        fr.readAsDataURL(f);
    }

    //hide warning
    $('span[data-valmsg-for="Photos"]').text('');
}

//add listner to first input element
var input_file = document.getElementsByClassName('files')[0];
input_file.addEventListener('change', showFile, false);

/* -------------------------------------------------------------------------*/



/* validation adding form*/

function validAddingForm(eventObject) {

    var inputPhotos = document.getElementsByClassName('files')[0];
    var photos = inputPhotos.files;

    var textarea = document.getElementById('DescribeText');
    var lengthTextarea = $(textarea).val().length;

    var inputName = document.getElementById('Name');
    var nameLength = $(inputName).val().length;

    var inputDate = document.getElementById('Date');
    var date = $(inputDate).val()

    if (lengthTextarea < 100 || nameLength < 2 || date == "") {
        if (nameLength < 2)
            $('span[data-valmsg-for="Name"]').text('Name must be set!');
        else
            $('span[data-valmsg-for="Name"]').text('');

        if (date == "")
            $('span[data-valmsg-for="Date"]').text('Date must be set!');
        else
            $('span[data-valmsg-for="Date"]').text('');

        //  if (photos.length == 0)
        //     $('span[data-valmsg-for="Photos"]').text('You must add photos!');


        if (lengthTextarea < 100)
            $('span[data-valmsg-for="Comment"]').text('The length of comment must be more than 100 characters!');
        else
            $('span[data-valmsg-for="Comment"]').text('');

        if (eventObject.preventDefault) {
            eventObject.preventDefault();

        } else if (window.event) /* for ie */ {
            window.event.returnValue = false;
        }
    }
}

var addingForm = window.document.getElementById("editing_form");
if (addingForm.addEventListener) {
    addingForm.addEventListener("submit", validAddingForm, false);
} else if (addingForm.attachEvent) {
    addingForm.attachEvent("onsubmit", validAddingForm);
}

/* -------------------------------------------------------------------------*/


/*for delete photos*/
function deletePhoto(e) {
    var photo = e.target;
    if (photo.style.border != "3px solid red")
    {
        photo.style.border = "3px solid red";
        $("#form0").append('<input id="url" name="url" type="hidden" value="' + photo.getAttribute('src') + '">');
    }
    else
    {
        photo.style.border = "";
        $("input[value='" + photo.getAttribute('src') + "']").remove();
    }

}

function initDeletePhoto(e) {
    var photos = $("img");

    for (var i = 0; i < photos.length; i++)
        photos[i].addEventListener("click", deletePhoto);
}

addEventListener('DOMContentLoaded', initDeletePhoto);
$(document).bind("ajaxComplete", initDeletePhoto);

/* -------------------------------------------------------------------------*/
