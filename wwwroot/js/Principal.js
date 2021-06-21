class Principal {
    executiveLink(URLactual) {
        let url = "";
        let cadena = URLactual.split("/");
        for (var i = 0; i < cadena.length; i++) {
            if (cadena[i] != "Index") {
                url += cadena[i];
            }
        }
        switch (url) {
            case "ExecutiveRegister":
                document.getElementById('files').addEventListener('change', imageExecutive, false);
                break;
        }
    }
}
