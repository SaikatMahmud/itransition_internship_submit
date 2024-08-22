window.addEventListener("DOMContentLoaded", function () {
    var preferredLang = "en";
/*    if (localStorage.lang === "bn") {
        preferredLang = "bn";
    }*/
    function switchLang(lang) {
        if (lang === "bn") {
            $('#switchlang').text("Eng");
            $('#switchlang').data("lang", lang);
        }
         $("[data-" + lang + "]").text(function (i, e) {
             return $(this).data(lang);
         });
         $("input[type='submit']").val(function () {
             return $(this).data(lang);
         });
    }

    switchLang(preferredLang);

    $("#switchlang").click(function () {
        alert("Language switch work in progress. Will be done by this weekend (before project defence).")
       /* $(this).text($(this).data("lang") == "bn" ? "বাং" : "Eng");
        var lang = $(this).data("lang") == "bn" ? "en" : "bn";
        $(this).data("lang", lang);
        localStorage.lang = lang;
        switchLang(lang)*/
    });
});