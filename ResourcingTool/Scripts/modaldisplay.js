$(".datarow").click(function () {

    var thisID = $(this).attr('id');
    var str = "modal" + thisID;

    console.log(str);

    modal = document.getElementById(str);
    modal.style.display = "block";

});

// Get the modal
var modal = document.getElementsByClassName("myModal");
// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];


// When the user clicks on <span> (x), close the modal
$(".close").click(function () {
    $('.myModal').css("display", "none");
});

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        $('.myModal').css("display", "none");
    }
}