$(document).ready(function () {
    document.SemesterShared = document.SemesterShared || [];
    console.log(document.SemesterShared.SemesterUpdatePartialViewUrl);

    $('#updateSemesterModalToggle').on('click', function (e) {
        var semesterId = $(this).attr('data-semester-id');
        $('#mainModalContent').load(document.SemesterShared.SemesterUpdatePartialViewUrl.replace('-1', semesterId));
        $('#mainModal').modal('show');
    });

    $('#addSemesterModalToggle').on('click', function (e) {
        $('#mainModalContent').load(document.SemesterShared.SemesterAddPartialViewUrl);
        $('#mainModal').modal('show');
    });
});