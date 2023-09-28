if (!window.location.search.includes("?")) {
    window.location = "https://localhost:44389/Home/Index";
}
let user = document.getElementById("user");
let iskustva = document.getElementById("iskustva");
let username = window.location.search.replace("?", "");
user.innerHTML = username;

fetch("https://localhost:44389/Experience/getExperiencesOfUser/" + username)
    .then(p =>
        p.json().then(data => {
            for (let i = 0; i < data.value.length; i++) {
                iskustva.innerHTML += napraviIskustvo(decodeLink(data.value[i].link), data.value[i].marka, data.value[i].godiste, data.value[i].opis);
            }
        })
    )

let iskustvo = `<div class="card card-svaisk">
                        <div class="card-header">
                            <ul class="nav nav-pills card-header-pills">
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

function napraviIskustvo(link, marka, godiste, opis) {
    let result = iskustvo.replace("$marka$", marka)
        .replace("$godiste$", godiste)
        .replace("$link$", link)
        .replace("$opis$", opis)
    return result;
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