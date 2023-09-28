let fIme = document.getElementById("ime");
let fPrezime = document.getElementById("prezime");
let fEmail = document.getElementById("email");
let fPasword = document.getElementById("password");
let username = document.getElementById("username");
let invalidInput = document.getElementById("invalid");

window.onload = this.getInfo();

let btnOtkazi = document.getElementById("otkazi");
btnOtkazi.onclick=(ev)=>this.getInfo();

let btnIzmeni = document.getElementById("izmeni")
btnIzmeni.onclick=(ev)=>this.izmeniInfo();

function getInfo() {
    fetch("https://localhost:44389/Home/getUserInfo/" + username.innerText)
    .then(result => result.json())
        .then(data => {
            fIme.value = data.ime;
            fPrezime.value = data.prezime;
            fEmail.value = data.email;
            fPasword.value = null;   
        }
    );

    fPasword.classList.remove("is-invalid");
    invalidInput.classList.add("d-none");
}

function izmeniInfo() {
    let errorBox = document.getElementById("error");

    fetch("https://localhost:44389/Home/editUserInfo/" + fIme.value + "/" + fPrezime.value + "/" + fEmail.value + "/" + fPasword.value + "/" + username.innerText, {
        method:"PUT"
    })
    .then(response => {
        if(response.status == 400) {
            errorBox.classList.remove("d-none");
            errorBox.innerHTML = "Email vec postoji!";    
        }

        if(response.status == 404) {
            fPasword.classList.add("is-invalid");
            invalidInput.classList.remove("d-none");
        }

        if(response.status == 200) {
            errorBox.classList.add("d-none");
            let successBox = document.getElementById("success");
            fPasword.classList.remove("is-invalid");
            invalidInput.classList.add("d-none");
            successBox.classList.remove("d-none");
            successBox.innerHTML = "Uspesna izmena podataka!";
            setTimeout(function() {
                location.reload();
            }, 2000)
        }
    })
}