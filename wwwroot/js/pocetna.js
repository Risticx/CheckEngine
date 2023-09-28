let nacinTrazenja = 1;
let vozila = document.getElementById("vozila");
fetch("https://localhost:44389/Experience/GetSomeExperiences/" )
    .then(p =>
        p.json().then(data => {
            let urls = [];
            for(let i = 0; i < data.length; i++) {
                vozila.innerHTML += napraviVozilo(decodeLink(data[i].link), data[i].marka, data[i].godiste, data[i].opis, data[i].username);
                urls.push(data[i].link);
            }

            return urls;
        })
    )
    .then(data => {
        for(let i = 0; i < data.length; i++) {
            fetch(" https://jsonlink.io/api/extract?url=" + decodeLink(data[i]) )
                .then(p =>
                    p.json().then(og => {
                        let img = og.images[0];

                        document.getElementById("vozilo-slika-"+i).src=img;
                    })
                );
        }
    }
);

function napraviVozilo(link, marka, godiste, opis, clan) {
    let result = vozilo.replace("$marka$", marka)
        .replace("$godiste$", godiste)
        .replace("$link$", decodeLink(link))
        .replace("$img_id$", img_counter++)
        .replace("$opis$", opis)
        .replace("$clan$", clan);
    return result;
}

let img_counter = 0;
let vozilo = `  <div class="col mb-5">
                    <div class="card h-100">
                        <img class="card-img-top" id="vozilo-slika-$img_id$" src="https://dummyimage.com/450x300/dee2e6/6c757d.jpg" alt="..." />
                        <div class="card-body p-4">
                            <div class="text-center">
                                <h5 class="fw-bolder" id="label11">Marka: $marka$</h5>
                                <p class="god fw-bolder">Godiste: $godiste$</p>
                                <div class="overflow-auto" id="velicina">
                                    $opis$ 
                                </div>
                                <a class="clan fw-bolder" onClick="getUsername(this.innerHTML)">$clan$</a>
                            </div>
                        </div>
                        <div class="card-footer p-4 pt-0 border-top-0 bg-transparent">
                            <div class="text-center"><a class="btn btn-outline-dark mt-auto" href="$link$" target="_blank">Pogledaj automobil</a></div>
                        </div>
                    </div>
                </div>`;

function getUsername(username) {
    window.location = "https://localhost:44389/Home/Iskustva?" + username;
    localStorage.setItem("username", username);
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

let btnDodaj = document.getElementById("sacuvaj");

fetch("https://localhost:44389/Home/getUsername/", {
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
})
    .then(p =>
        p.json().then(data => {
            btnDodaj.onclick = (ev) => this.dodajIskustvo(data);
}))

function dodajIskustvo(username) {
    console.log(username);
    btnDodaj.classList.add("disabled");
    let link = document.getElementById("linkDodaj").value;
    let marka = document.getElementById("FormControlSelect11").value;
    let godiste = document.getElementById("FormControlSelect12").value;
    let opis = document.getElementById("FormControlTextarea11").value;

    let errorBox = document.getElementById("error");
    let successBox = document.getElementById("success");

    fetch("https://localhost:44389/Experience/checkExperience/" + codeLink(link) + "/" + username)
        .then(response => {
            if (response.status == 200) {
                fetch("https://localhost:44389/Experience/addExperience/" + codeLink(link) + "/" + marka + "/" + godiste + "/" + opis + "/" + username,
                    {
                        method: "POST"
                    }).then(response => {

                        if (response.status == 400) {
                            btnDodaj.classList.remove("disabled");
                            errorBox.classList.remove("d-none");
                            errorBox.innerHTML = "Greska";
                        }

                        if (response.status == 404) {
                            btnDodaj.classList.remove("disabled");
                            errorBox.classList.remove("d-none");
                            errorBox.innerHTML = "Morate popuniti sva polja";
                        }

                        if (response.status == 200) {
                            btnDodaj.classList.add("disabled");
                            successBox.classList.remove("d-none");
                            errorBox.classList.add("d-none");
                            successBox.innerHTML = "Uspesno dodato iskustvo";
                            setTimeout(function () {
                                location.reload();
                            }, 2000)
                        }
                    })
            }
            else if (response.status != 200) {
                errorBox.classList.remove("d-none");
                errorBox.innerHTML = "Vec ste uneli iskustvo za ovo vozilo!";
                btnDodaj.classList.remove("disabled");
            }
        })
}

let parentDiv = document.getElementById("LinkSearchDiv");
let dropMenuLink = document.getElementById("link");
let dropMenuCombined = document.getElementById("combined");

let Combined = document.getElementById("MadeAndYearSearchDiv");
dropMenuLink.onclick = (ev) => this.izaberiLink();
dropMenuCombined.onclick = (ev) => this.izaberiCombined();

let autoSlika = document.getElementById("autoSlika");
function izaberiLink() {
    parentDiv.classList.remove("d-none");
    Combined.classList.add("d-none");
    nacinTrazenja == 1;
    autoSlika.classList.remove("d-none");
}

function izaberiCombined() {
    parentDiv.classList.add("d-none");
    Combined.classList.remove("d-none");
    nacinTrazenja == 2;
    autoSlika.classList.add("d-none");
}

let btnPretrazi = document.getElementById("btnPretrazi");
let btnPretrazii = document.getElementById("btnPretrazii");
btnPretrazi.onclick = (ev) => this.nadjiIskustvo();
btnPretrazii.onclick = (ev) => this.nadjiIskustvoPoModeluIGodistu();

function nadjiIskustvoPoModeluIGodistu() {
    let modalBody = document.getElementById("iskustvoModal");
    modalBody.innerHTML = "";
    errorYear = document.getElementById("error-year");

    let marka = document.getElementById("markaSearch").value;
    let godiste = document.getElementById("godisteSearch").value;
    fetch("https://localhost:44389/Experience/experienceExistMadeAndYear/" + marka + "/" + godiste)
        .then(response => {
            if (response.status == 200) {
                errorYear.classList.add("d-none");
                fetch("https://localhost:44389/Experience/getExperiencseByMadeAndYear/" + marka + "/" + godiste)
                    .then(p =>
                        p.json().then(data => {
                            document.getElementById("marka").innerHTML = "Marka: " + data.value[0].marka;
                            document.getElementById("godiste").innerHTML = "Godiste: " + data.value[0].godiste;
                            for (let i = 0; i < data.value.length; i++) {
                                modalBody.innerHTML += napraviExperienceMY(data.value[i].opis, data.value[i].username, decodeLink(data.value[i].link));
                            }
                            $("#myModal").modal("toggle");

                        })
                    )
            }
            else {
                errorYear = document.getElementById("error-year");
                errorYear.innerHTML = "Ne postoji nijedno iskustvo za takvo vozilo!";
                errorYear.classList.remove("d-none");
            }
        })
}


function nadjiIskustvo() {
    let modalBody = document.getElementById("iskustvoModal");
    modalBody.innerHTML = "";
    let link = document.getElementById("searchBox");
    let invalidInput = document.getElementById("invalid");
    errorLink = document.getElementById("error-link");
    if (link.value == "") {

        errorLink.innerHTML = "Polje ne sme biti prazno!";
        errorLink.classList.remove("d-none");
    } else {
        fetch("https://localhost:44389/Experience/experienceExist/" + codeLink(link.value))
            .then(response => {
                if (response.status == 200) {

                    fetch("https://localhost:44389/Experience/getExperiences/" + codeLink(link.value))
                        .then(p =>
                            p.json().then(data => {
                                if (data.value[0].link != null) {
                                    fetch(" https://jsonlink.io/api/extract?url=" + decodeLink(data.value[0].link))
                                        .then(p => p.json().then(og => {
                                            document.getElementById("autoSlika").src = og.images;
                                        }))
                                    document.getElementById("marka").innerHTML = "Marka: " + data.value[0].marka;
                                    document.getElementById("godiste").innerHTML = "Godiste: " + data.value[0].godiste;
                                    document.getElementById("linkVozila").href = decodeLink(data.value[0].link);
                                    errorLink.classList.add("d-none");


                                    for (let i = 0; i < data.value.length; i++) {
                                        modalBody.innerHTML += napraviExperience(data.value[i].opis, data.value[i].username);
                                    }
                                    $("#myModal").modal("toggle");
                                }
                            })
                        )
                }
                else if (response.status != 200) {
                    errorLink = document.getElementById("error-link");
                    errorLink.innerHTML = "Ne postoji nijedno iskustvo za takvo vozilo!";
                    errorLink.classList.remove("d-none");
                }
            })
    }
}

var experience = `  <div>
                        <hr>
                            <li class="fw-bold">$opis$</li>
                            <p class="text-muted">
                                Iskustvo objavio: <a href="/Home/Iskustva?$username$">$username$</a>
                            </p>
                            
                    </div>`;
var experienceMY = `  <div>
                        <hr>
                            <li class="fw-bold">$opis$</li>
                            <p class="text-muted">
                                Iskustvo objavio: <a href="/Home/Iskustva?$username$">$username$</a>
                                <br><a href="$link$">Link vozila</a>
                            </p>
                            
                    </div>`;

function napraviExperienceMY(opis, username, link) {
    let result = experienceMY.replace("$opis$", opis)
        .replaceAll("$username$", username)
        .replaceAll("$link$", link);
    return result;
}


function napraviExperience(opis, username) {
    let result = experience.replace("$opis$", opis)
        .replaceAll("$username$", username);
    return result;
}