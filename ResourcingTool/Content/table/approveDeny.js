$(".approveRow").click(function () {

    var thisID = $(this).parent().parent().attr('id');
    var str = "" + thisID;

    console.log(str);

    $("#approveID").val(thisID);

    $('#approveForm').submit();
});

$(".denyRow").click(function () {

    var thisID = $(this).parent().parent().attr('id');
    var str = "" + thisID;

    console.log(str);

    $("#denyID").val(thisID);


    $('#denyForm').submit();
});