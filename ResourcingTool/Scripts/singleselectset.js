$("#techTable").hide();

$("#option-0").click(function () {
    $("#ProjectSector").val("PwC");
    console.log("0");
});
$("#option-1").click(function () {
    $("#ProjectSector").val("FS");
    console.log("1");
});
$("#option-2").click(function () {
    $("#ProjectSector").val("C&BS");
    console.log("2");
});
$("#option-3").click(function () {
    $("#TechRequired").val("true");
    $("#techTable").show();

    $("#DCTotalM").prop('required', true);
    $("#DCTotalSA2").prop('required', true);
    $("#DCTotalSA1").prop('required', true);
    $("#DCTotalA2").prop('required', true);
    $("#DCTotalA1").prop('required', true);
    $("#DATotalM").prop('required', true);
    $("#DATotalSA2").prop('required', true);
    $("#DATotalSA1").prop('required', true);
    $("#DATotalA2").prop('required', true);
    $("#DATotalA1").prop('required', true);
    $("#DVTotalM").prop('required', true);
    $("#DVTotalSA2").prop('required', true);
    $("#DVTotalSA1").prop('required', true);
    $("#DVTotalA2").prop('required', true);
    $("#DVTotalA1").prop('required', true);
});
$("#option-4").click(function () {
    $("#TechRequired").val("false");
    $("#techTable").hide();

    $("#DCTotalM").prop('required', false);
    $("#DCTotalSA2").prop('required', false);
    $("#DCTotalSA1").prop('required', false);
    $("#DCTotalA2").prop('required', false);
    $("#DCTotalA1").prop('required', false);
    $("#DATotalM").prop('required', false);
    $("#DATotalSA2").prop('required', false);
    $("#DATotalSA1").prop('required', false);
    $("#DATotalA2").prop('required', false);
    $("#DATotalA1").prop('required', false);
    $("#DVTotalM").prop('required', false);
    $("#DVTotalSA2").prop('required', false);
    $("#DVTotalSA1").prop('required', false);
    $("#DVTotalA2").prop('required', false);
    $("#DVTotalA1").prop('required', false);
});