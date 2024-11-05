
window.addEventListener('DOMContentLoaded', async () => {
    const userPreferredLanguage = localStorage.getItem('language') || 'en';
    const langData = await fetchLanguageData(userPreferredLanguage);
    updateContent(langData);
    $(".switchlang").show();
    $(`#${userPreferredLanguage}Btn`).hide();
    $(".switchlang").click(function () {
        var lang = $(this).val();
        changeLanguage(lang);
        $(".switchlang").show();
        $(this).hide();
    });

});
async function fetchLanguageData(lang) {
    const response = await fetch(`language/${lang}.json`);
    return response.json();
}
function updateContent(langData) {
    document.querySelectorAll('[data-i18n]').forEach(element => {
        const key = element.getAttribute('data-i18n');
        if (element.tagName === 'INPUT') {
            element.value = langData[key];
        } else {
            element.textContent = langData[key];
        }
    });
}

async function changeLanguage(lang) {
    await setLanguagePreference(lang);
    const langData = await fetchLanguageData(lang);
    updateContent(langData);    
}

function setLanguagePreference(lang) {
    localStorage.setItem('language', lang);
}