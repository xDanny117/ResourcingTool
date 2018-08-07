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

$(".approveRow2").click(function () {

    var thisID = $(this).parent().parent().parent().attr('id');

    console.log(thisID);

    var str = thisID.substring(5, thisID.length);

    console.log(str);

    $("#approveID").val(str);

    $('#approveForm').submit();
});

$(".denyRow2").click(function () {

    var thisID = $(this).parent().parent().parent().attr('id');

    console.log(thisID);

    var str = thisID.substring(5, thisID.length);

    console.log(str);

    $("#denyID").val(str);


    $('#denyForm').submit();
});


$(".editRow").click(function () {

    var thisID = $(this).parent().parent().attr('id');
    var str = "" + thisID;

    console.log(str);

    $("#denyID").val(thisID);


    $('#denyForm').submit();
});