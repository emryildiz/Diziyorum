const hamburger = document.querySelector(".hamburger");
const navLinks = document.querySelector(".nav-links");
const links = document.querySelector(".navlinks li");


hamburger.addEventListener("click", ()=>{
    navLinks.classList.toggle("open");

 });    