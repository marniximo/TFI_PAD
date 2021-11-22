var error = { };

function mostrarError() {
    var domError = $("#error");
    domError.html(error.mensaje);
    domError.css("display", "block");
}

function validarRegistro() {
    let password = $("#password").val();
    let confirm = $("#confirm").val();
    if (password.length < 8) {
        error.mensaje = "La contraseña debe tener al menos 8 caracteres!";
        mostrarError();
        return false;
    }
    if (password != confirm) {
        error.mensaje = "Las contraseñas no coinciden!";
        mostrarError();
        return false;
    }
    return true;
}