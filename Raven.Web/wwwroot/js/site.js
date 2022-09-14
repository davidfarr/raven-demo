//var WebPort = '49185';
//var ProjectsPort = '49233';
//var DesignPort = '49189';
//var TestingPort = '7099';


$(document).ready(function () {
    GetProjects();
});

var activeProjectId = '';

/*-------- Projects -----------*/
function GetProjects() {
    $('#projectSelect > div > div > button').remove();
    $('.project-buttons').append('<div class="spinner-border" role="status"></div>');
    $.ajax({
        method: 'GET',
        url: 'https://localhost:49233/projects/shortlist',
        success: function (data) {
            $('.spinner-border').remove();
            for (let i = 0; i < data.length; i++) {
                $('.project-buttons').append('<button type="button" '
                    + 'class="btn btn-outline-light btn-sm" onclick="OpenRequirements(\''
                    + data[i].projectId
                    + '\')">'
                    + data[i].shortCode
                    + '</button>'
                );
            }
        }
    });
}

$('#saveProject').on('click', function (e) {
    e.preventDefault();
    $('#projCreate').append('<div class="spinner-border" role="status"></div>');
    $.ajax({
        method: 'POST',
        url: 'https://localhost:49233/projects/create',
        data: JSON.stringify({ "title": document.getElementById("projTitle").value, "info": document.getElementById("projInfo").value, "shortCode": document.getElementById("shortCode").value }),
        dataType: 'json',
        contentType: 'application/json',
        success: function () {
            $('.spinner-border').remove();
            document.getElementById("projTitle").value = '';
            document.getElementById("projInfo").value = '';
            document.getElementById("shortCode").value = '';
            $('#projCreate').hide();
            GetProjects();
        }
    });
});

$('#selectNewProject').on('click', function () {
    $('#projectSelect').show();
    $('#projectBack').hide();
    document.getElementById('details-title').innerText = '';
    document.getElementById('details-shortcode').innerText = '';
    document.getElementById('details-info').innerText = '';
    document.getElementById('details-created').innerText = '';
    document.getElementById('details-updated').innerText = '';
    $('#reqsTable tbody tr').remove();
    $('#projectDetails').hide();
    $('#requirementSelect').hide();
});

$('#cancelNewProject').on('click', function () {
    document.getElementById("projTitle").value = '';
    document.getElementById("projInfo").value = '';
    document.getElementById("shortCode").value = '';
    $('#projCreate').hide();
})

function NewProj() {
    $('#projSelect').hide();
    $('#projCreate').show();
}

/*-------- Requirements -----------*/
$('#newrequirement').on('click', function (e) {
    let projId = $('#projectIdentifier').val();
    $('#newReqmodal .modal-footer').append('<div class="spinner-border" role="status"></div>');
    $.ajax({
        method: 'POST',
        url: 'https://localhost:49237/design/create',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            "title": document.getElementById("reqtext").value,
            "info": document.getElementById("reqinfo").value,
            "versionIntroduced": document.getElementById("reqversion").value,
            "projectId": projId
        }),
        success: function () {
            document.getElementById("reqtext").value = '';
            document.getElementById("reqinfo").value = '';
            document.getElementById("reqversion").value = '';
            document.getElementById('details-title').innerText = '';
            document.getElementById('details-shortcode').innerText = '';
            document.getElementById('details-info').innerText = '';
            document.getElementById('details-created').innerText = '';
            document.getElementById('details-updated').innerText = '';
            $('.spinner-border').remove();
            $('#newReqmodal .modal-footer .btn-secondary').click();
            OpenRequirements(projId);
        }
    });
});

function OpenRequirements(projId) {
    $('#projectBack').show();
    $('#requirementSelect').show();
    $('#reqsTable > tbody').empty();
    $('#projectSelect').hide();
    $('#projCreate').hide();
    $('#projectDetails').show();
    $('#projectDetails .app-div').append('<div class="spinner-border" role="status"></div>');
    $.ajax({
        method: 'GET',
        url: ('https://localhost:49233/projects/' + projId),
        success: function (data) {
            $('#details-title').append(data.title);
            $('#details-shortcode').append(data.shortCode);
            $('#details-info').append(data.info);
            $('#details-created').append(data.createdDate);
            $('#details-updated').append(data.updatedDate);
            $('.spinner-border').remove();
            $('#projectIdentifier').val(projId);
        }
    });
    $.ajax({
        method: 'GET',
        url: ('https://localhost:49237/design/project/' + projId),
        success: function (data) {
            for (let i = 0; i < data.length; i++) {
                var covered = data[i].testCase === null;
                $('#reqsTable > tbody:last-child').append(
                    '<tr><td>' + data[i].title + '</td><td>' + data[i].versionIntroduced + '</td><td>' + data[i].createdDate + '</td><td>' + (data[i].updatedDate === null ? '' : data[i].updatedDate) + '</td><td><span class="cell-' + (covered ? 'ok' : 'alert') + '">' + (covered ? 'Covered' : 'Uncovered') + '</span></td><td>'
                    + '<button type="button" class="btn btn-sm btn-primary" data-bs-toggle="modal" data-val="' + data[i].requirementId + '" data-bs-target="#tcModal">Expand <i class="bi bi-box-arrow-in-up-right"></i></button></td></tr>'
                );
            }
        }
    });
}

$('#tcModal').on('show.bs.modal', function (e) {
    let reqId = $(e.relatedTarget).data('val');
    $.ajax({
        method: 'GET',
        url: ('https://localhost:49237/design/requirement/' + reqId),
        success: function (data) {
            $('#reqdetail-title-header').append(data.title);
            $('#reqdetail-created').append(data.createdDate);
            $('#reqdetail-updated').append(data.updatedDate);
            $('#reqdetail-title').val(data.title);
            $('#reqdetail-info').val(data.info);
            $('#reqdetail-version').val(data.versionIntroduced);
            $('#reqIdentifier').val(data.requirementId);
        }
    });
    LoadTestCases(reqId);
});

$('#updaterequirement').on('click', function () {
    console.log("here");
    let projId = $('#projectIdentifier').val();
    console.log("projId: " + projId);
    let reqId = $('#reqIdentifier').val();
    console.log("reqId: " + reqId);
    $.ajax({
        method: 'POST',
        url: ('https://localhost:49237/design/update'),
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            "title": document.getElementById("reqdetail-title").value,
            "info": document.getElementById("reqdetail-info").value,
            "versionIntroduced": document.getElementById("reqdetail-version").value,
            "requirementId": reqId,
            "projectId": projId
        }),
        success: function () {           
            $('#tcModal .modal-footer .btn-secondary').click();
            OpenRequirements(projId);
        }
    });
});

$('#tcModal').on('hide.bs.modal', function (e) {
    $('#reqdetail-title').val('');
    $('#reqdetail-created').text('')
    $('#reqdetail-updated').text('')
    $('#reqdetail-title').val('');
    $('#reqdetail-info').val('');
    $('#reqdetail-version').val('');
    $('#tcTable > tbody').empty();
    $('#reqdetail-title-header').value = '';
    $('#createTc').hide();
    $('#reqIdentifier').val('');
    $('#caseIdentifier').val('');
});

/*-------- Test Cases -----------*/

function LoadTestCases(reqId) {
    $.ajax({
        method: 'GET',
        url: ('https://localhost:49239/testing/requirement/' + reqId),
        success: function (data) {
            for (let i = 0; i < data.length; i++) {
                var covered = data[i].testCase === null;
                $('#tcTable > tbody:last-child').append(
                    '<tr><td>' + data[i].title + '</td><td>' + data[i].createdDate + '</td><td>' + (data[i].updatedDate === 'null' ? '' : data[i].updatedDate) +
                    '</td><td><a href="#" onclick="OpenTestCase(\'' + data[i].testCaseId + '\')" class="btn btn-sm btn-primary">Open <i class="bi bi-caret-right-fill"></i></a></td></tr>'
                    //'</td><td><button type="button" class="btn btn-sm btn-primary open-case" data-val="' + data[i].testCaseId + '">Open <i class="bi bi-caret-right-fill"></i></button></td></tr>'
                );
            }
        }
    });
}

function SetupTestCase() {
    $('.createTC').show();
    $('.testCasesTable').hide();
    $('.newTcTitle').show();
}

function OpenTestCase(caseId) {
    $('.createTC').show();
    $('.testCasesTable').hide();
    $('.editTcTitle').show();
    $('#caseIdentifier').val(caseId);
    $.ajax({
        method: 'GET',
        url: ('https://localhost:49239/testing/' + caseId),
        success: function (data) {
            $('#tcTitle').val(data.title);
            $('#tcInfo').val(data.info);
        }
    });
}

function TeardownTestCase() {
    $('#tcTitle').val('');
    $('#tcInfo').val('');
    $('.newTcTitle').hide();
    $('.createTC').hide();
    $('.testCasesTable').show();
    $('#tcTable > tbody').empty();
    $('#caseIdentifier').val('');
    LoadTestCases($('#reqIdentifier').val());
}

$('#saveTC').on('click', function () {
    let reqId = $('#reqIdentifier').val();
    let projId = $('#projectIdentifier').val();
    let caseId = $('#caseIdentifier').val();

    if (caseId !== null && caseId !== '') {
        $.ajax({
            method: 'POST',
            url: 'https://localhost:49239/testing/update',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({
                "title": document.getElementById("tcTitle").value,
                "info": document.getElementById("tcInfo").value,
                "requirementId": reqId,
                "projectId": projId,
                "testCaseId": caseId
            }),
            success: function () {
                TeardownTestCase();
            }
        });
    }
    else {
        $.ajax({
                method: 'POST',
                url: 'https://localhost:49239/testing/create',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    "title": document.getElementById("tcTitle").value,
                    "info": document.getElementById("tcInfo").value,
                    "requirementId": reqId,
                    "projectId": projId
                }),
                success: function () {
                    TeardownTestCase();
                }
        });
    }
  
});