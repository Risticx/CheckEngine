let btn = document.getElementById("reg");
btn.onclick = (ev) => this.registruj();

function registruj() {
    let ime = document.getElementById("ime").value;
    let prezime = document.getElementById("prezime").value;
    let email = document.getElementById("email").value;
    let user = document.getElementById("user").value;
    let pass = document.getElementById("pass").value;

    let errorBox = document.getElementById("error");
    let successBox = document.getElementById("success");

    fetch("https://localhost:44389/Home/Register/" + user + "/" + pass + "/" + ime + "/" + prezime + "/" + email, {
        method : "POST"
    }).then(response => {

        if(response.status == 400) {
            errorBox.classList.remove("d-none");
            errorBox.innerHTML = "Korisnik je vec registrovan!";
        }

        if(response.status == 404) {
            errorBox.classList.remove("d-none");
            errorBox.innerHTML = "Morate popuniti sva polja!";
        }

        if(response.status == 200) {
            successBox.classList.remove("d-none");
            errorBox.classList.add("d-none");
            successBox.innerHTML = "Uspesna registracija!";
            setTimeout(function() {
                window.location = 'https://localhost:44389/Home/Login?ReturnUrl=%2fHome%2fIndex';
            },2000)     
        }
    });
}