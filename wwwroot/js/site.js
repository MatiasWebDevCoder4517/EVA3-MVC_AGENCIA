// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var principal = new Principal();


/*CODIGO DE Ejecutivos*/
var executives = new Executives();
var imageExecutive = (evt) => {
    executive.archivo(evt, "imageExecutive");
}

/*CODIGO DE CLIENTES*/
var clients = new Clients();
var imageClients = (evt) => {
    clients.archivo(evt, "imageClients");
}

$().ready(() => {
    let URLactual = window.location.pathname;
    principal.executiveLink(URLactual);
});