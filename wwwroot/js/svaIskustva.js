let user = document.getElementById("user");
let iskustva = document.getElementById("iskustva");
let username = window.location.search.replace("?", "");

let IDs = [];
  
fetch("https://localhost:44389/Experience/getExperiencesOfUser/" + user.innerText)
    .then(p =>
        p.json().then(data => {
            for (let i = 0; i < data.value.length; i++) {
                iskustva.innerHTML += napraviIskustvo(decodeLink(data.value[i].link), data.value[i].marka, data.value[i].godiste, data.value[i].opis, i, data.value[i].id);
                IDs.push(data.value[i].id);
            }
        })
    )

let iskustvo = `<div class="card card-svaisk" id="d$i$">
                        <div class="card-header">
                            <ul class="nav nav-pills card-header-pills">
                                <li class="nav-item text-right">
                                    <button class="btn btn-danger" id="b$i$" onClick="obrisiIskustvo(this.id)">Obriši iskustvo</button> 
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="$link$">Link vozila</a>
                                </li>
                                </li>
                            </ul>
                        </div>
                        <div class="card-body">
                            <h5 class="card-title">$marka$ - $godiste$</h5>
                            <p class="card-text" id="o$i$">$opis$</p>
                        </div>
                    </div>`;

function napraviIskustvo(link, marka, godiste, opis, i, id) {
    let result = iskustvo.replace("$marka$", marka)
        .replace("$godiste$", godiste)
        .replace("$link$", link)
        .replace("$opis$", opis)
        .replaceAll("$i$", i)
        .replace("$l$", i)
        .replace("$ID$", id)
    return result;
}

function obrisiIskustvo(clicked_button) {
    let buttonClicked = clicked_button.replace("b", '');

    fetch("https://localhost:44389/Experience/deleteExperience/" + IDs[buttonClicked] + "/" + user.innerText, {
        method: "DELETE"
    })

    let divToRemove = document.getElementById("d" + buttonClicked);
    divToRemove.parentNode.removeChild(divToRemove);
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