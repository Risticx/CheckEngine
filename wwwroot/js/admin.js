let errorBoxDelete = document.getElementById("errorDelete");
let successBoxDelete = document.getElementById("successDelete");

let btnObrisiKorisnika = document.getElementById("btnObrisiKorisnika");
let btnObrisiIskustvo = document.getElementById("btnArhivirajIskustvo");
let btnUnArchive = document.getElementById("btnUnArchive")

btnObrisiKorisnika.onclick=(ev)=>this.obrisiKorisnika();

function obrisiKorisnika() {
    let username = document.getElementById("username").value;

    fetch("https://localhost:44389/Admin/deleteUser/" + username, {
        method : "DELETE"
    })
    .then(response => {
        if(response.status == 200) {
            successBoxDelete.classList.remove("d-none");
            errorBoxDelete.classList.add("d-none");
            successBoxDelete.innerHTML = "Uspesno obrisan korisnik!";
            setTimeout(function() {
                errorBoxDelete.classList.add("d-none");
                successBoxDelete.classList.add("d-none");
            },4000)  
        }

        if(response.status == 400) {
            errorBoxDelete.classList.remove("d-none");
            errorBoxDelete.innerHTML = "Korisnik ne postoji!";
        }

        if(response.status == 404) {
            errorBoxDelete.classList.remove("d-none");
            errorBoxDelete.innerHTML = "Morate popuniti sva polja!";
        }
    })
}

let errorBoxArchive = document.getElementById("errorArchive");
let successBoxArchive = document.getElementById("successArchive");

btnArhivirajIskustvo.onclick=(ev)=>this.arhivirajIskustvo();

function arhivirajIskustvo() {
    let username = document.getElementById("usernameIskustvo").value;
    let link = document.getElementById("link").value;

    fetch("https://localhost:44389/Admin/archiveExperience/" + codeLink(link) + "/" + username, {
        method : "PUT"
    })
    .then(response => {
        if(response.status == 200) {
            successBoxArchive.classList.remove("d-none");
            errorBoxArchive.classList.add("d-none");
            successBoxArchive.innerHTML = "Uspesno arhivirano iskustvo!";
            setTimeout(function() {
                errorBoxArchive.classList.add("d-none");
                successBoxArchive.classList.add("d-none");
            },4000)  
        }
        if(response.status == 400) {
            errorBoxArchive.classList.remove("d-none");
            errorBoxArchive.innerHTML = "Iskustvo ne postoji!";
        }

        if(response.status == 404) {
            errorBoxArchive.classList.remove("d-none");
            errorBoxArchive.innerHTML = "Morate popuniti sva polja!";
        }
    })
}


btnUnArchive.onclick=(ev)=>this.prikazujIskustvo();
function prikazujIskustvo() {

    let username = document.getElementById("usernameIskustvo").value;
    let link = document.getElementById("link").value;

    fetch("https://localhost:44389/Admin/unArchiveExperience/" + codeLink(link) + "/" + username, {
        method : "PUT"
    })
    .then(response => {
        if(response.status == 200) {
            successBoxArchive.classList.remove("d-none");
            errorBoxArchive.classList.add("d-none");
            successBoxArchive.innerHTML = "Uspesno odarhivirano iskustvo!";
            setTimeout(function() {
                errorBoxArchive.classList.add("d-none");
                successBoxArchive.classList.add("d-none");
            },4000)  
        }

        if(response.status == 400) {
            errorBoxArchive.classList.remove("d-none");
            errorBoxArchive.innerHTML = "Iskustvo ne postoji ili je vec vidljivo!";
        }

        if(response.status == 404) {
            errorBoxArchive.classList.remove("d-none");
            errorBoxArchive.innerHTML = "Morate popuniti sva polja!";
        }
    })
}   

function codeLink(link) {
    let replaced = link.split("/").join("%2F");
    let replaced2 = replaced.split("?").join("%3F");
    return replaced2;
}

function decodeLink(link) {
    let replaced = link.split("%2F").join("/");
    let replaced2 = replaced.split("%3F").join("?");
    return replaced2;
}