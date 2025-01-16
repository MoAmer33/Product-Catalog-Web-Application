// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function zoomImageOnHover() {
    const images = document.querySelectorAll('.ZoomHover');
    images.forEach(image => {
        image.addEventListener('mouseover', function () {
            image.style.transform = 'scale(1.1)';
            image.style.transition = 'transform 0.3s ease';
        });
        image.addEventListener('mouseout', function () {
            image.style.transform = 'scale(1)';
        });
    });
}

// Call function on page load
window.onload = zoomImageOnHover;