
$(document).ready(function () {
    $(".completedFilter").click(function () {
        $(".completedRow").fadeIn();
        $(".inprogressRow").fadeOut();
        $(".submittedRow").fadeOut();

        $(".filterButton").removeClass("active");
        $(this).addClass("active");
    });

    $(".unassignedFilter").click(function () {
        $(".completedRow").fadeOut();
        $(".inprogressRow").fadeOut();
        $(".submittedRow").fadeIn();

        $(".filterButton").removeClass("active");
        $(this).addClass("active");
    });

    $(".inprogressFilter").click(function () {
        $(".completedRow").fadeOut();
        $(".inprogressRow").fadeIn();
        $(".submittedRow").fadeOut();

        $(".filterButton").removeClass("active");
        $(this).addClass("active");
    });
    
    $(".allFilter").click(function () {
        $(".completedRow").fadeIn();
        $(".inprogressRow").fadeIn();
        $(".submittedRow").fadeIn();

        $(".filterButton").removeClass("active");
        $(this).addClass("active");
    });
});
