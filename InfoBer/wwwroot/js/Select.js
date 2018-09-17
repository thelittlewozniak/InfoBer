
function giveSelection(selValue) {
    var options2 = document.getElementsByClassName('test');
    console.log(options2);
    for (var j = 0; j < options2.length; j++) {
        if (options2[j].style.display != "none") {
            options2[j].style.display = "none";
            console.log(options2[j]);
        }
        }

    for (var i = 0; i < options2.length; i++) {
        if (options2[i].dataset.option == selValue) {
            options2[i].style.display = "block";
            console.log(options2[i]);
        }
    }
}