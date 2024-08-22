const lightIcon = document.getElementById("light-icon");
const darkIcon = document.getElementById("dark-icon");
var darkMode = false;
var storedTheme;

var getStoredTheme = () => localStorage.getItem('theme');

window.addEventListener('DOMContentLoaded', () => {
    storedTheme = getStoredTheme();
    if (storedTheme === 'dark') {
        document.documentElement.setAttribute('data-bs-theme', storedTheme);
        darkIcon.style.display = "none";
        lightIcon.style.display = "block";
        darkMode = true;
    }
    else {
        lightIcon.style.display = "none";
        darkIcon.style.display = "block";
    }
});

document.getElementById('btnTheme').addEventListener('click', () => {
    toggleThemeMode();
});
function toggleThemeMode() {
    if (darkMode) {
        document.documentElement.setAttribute('data-bs-theme', 'light');
        darkIcon.style.display = "block";
        lightIcon.style.display = "none";
        localStorage.setItem('theme', 'light');
    }
    else {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
        lightIcon.style.display = "block";
        darkIcon.style.display = "none";
        localStorage.setItem('theme', 'dark');
    }
    darkMode = !darkMode;
};