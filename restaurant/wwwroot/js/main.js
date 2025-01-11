
let activePage = window.location.href;
console.log(activePage);
activePage = activePage.slice(23);

console.log(activePage);

while(activePage.includes('/')) {
    console.log(activePage.indexOf('/'));
    activePage = activePage.slice(0, activePage.lastIndexOf('/'));
}
console.log(activePage);
activePage = activePage.slice(0, -1);
console.log(activePage);

links = document.querySelectorAll('li span');

for (let i = 0; i < links.length; i++) {
    if (links[i].parentElement.parentElement.classList.contains("active"))
        links[i].parentElement.parentElement.classList.remove("active");

    if (links[i].innerHTML.toLowerCase() == activePage.toLowerCase() || (links[i].innerHTML == "Aperçu" && activePage.toLowerCase() == "apercu") || (links[i].innerHTML == "paramètres" && activePage.toLowerCase() == "parametre"))
        links[i].parentElement.parentElement.classList.add("active");


}

document.querySelector(".headerPageTitle").innerHTML = activePage;




