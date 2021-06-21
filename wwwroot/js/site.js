// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var principal = new Principal();


/*CODIGO DE Ejecutivos*/
var executive = new Executive();
var imageExecutive = (evt) => {
    executive.archivo(evt, "imageExecutive");
}

$().ready(() => {
    let URLactual = window.location.pathname;
    principal.executiveLink(URLactual);
});