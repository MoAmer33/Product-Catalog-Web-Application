// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


var ImageFile = document.getElementById("LoadImage");
var displayImage = document.getElementById("SelectedImage");

ImageFile.addEventListener("change", (event) => {
    var reader = new FileReader()

    if (event.target.files[0]) {

        reader.onload = (event) => {
            displayImage.src =event.target.result;
        }
    }
        reader.readAsDataURL(event.target.files[0]);

});



// Call function on page load
