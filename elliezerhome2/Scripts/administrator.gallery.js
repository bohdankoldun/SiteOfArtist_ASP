

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

//add listner to first input element
var input_file = document.getElementsByClassName('files')[0];
input_file.addEventListener('change', showFile, false);
/* -------------------------------------------------------------------------*/


/*for delete photos*/
function deletePhoto(e) {
    var photo = e.target;
    if (photo.style.border != "3px solid red") {
        photo.style.border = "3px solid red";
        $("#editing-form-gallery").append('<input id="url" name="url" type="hidden" value="' + photo.getAttribute('src') + '">');
    }
    else {
        photo.style.border = "";
        $("input[value='" + photo.getAttribute('src') + "']").remove();
    }

}

//initDeletePhoto(e) 
var photos = $("img");

for (var i = 0; i < photos.length; i++)
    photos[i].addEventListener("click", deletePhoto);

/* -------------------------------------------------------------------------*/


var addingForm = window.document.getElementById("adding-form-gallery");

if (addingForm.addEventListener) {
    addingForm.addEventListener("reset", function (e) {
        document.getElementById("images-line").innerHTML = "";
    });
} else if (addingForm.attachEvent) {
    addingForm.addEventListener("onreset", function (e) {
        document.getElementById("images-line").innerHTML = "";
    });
}



/* -------------------------------------------------------------------------*/
