var ApplicationRootUrl = 'https://localhost:';
var WebPort = '49185';
var ProjectsPort = '49187';
var DesignPort = '49189';
var TestingPort = '7099';


//$(document).ready(function () {
//    GetProjects();
//});

function GetProjects() {
    $.ajax({
        method: 'GET',
        url: (ApplicationRootUrl + ProjectsPort + '/projects/shortlist'),
        success: function (data) {
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
    console.log("here");
    var project = {
        Title: $('#projTitle').text(),
        ShortCode: $('#shortCode').text(),
        Description: $('#projInfo').text()
    };
    console.log(project);
});

function OpenRequirements(projId) {
    $('#projectBack').show();
    $('#requirementSelect').show();
    $('#projSelect').hide();
}

function NewProj() {
    $('#projSelect').hide();
    $('#projCreate').show();
}