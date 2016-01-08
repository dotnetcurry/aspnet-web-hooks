(function () {
    $(function () {
        var table = $("#tblGithubActivities");
        var lastActivityId;

        loadActivities().then(appendRows);

        setInterval(function () {
            loadActivitiesAfter(lastActivityId).then(function (data) {
                if (data.length > 0) {
                    appendRows(data);
                }
            });
        }, 2*60*1000);

        function appendRows(data) {
            lastActivityId = data[data.length-1].ActivityId;

            var rows = "";            
            data.forEach(function (entry) {
                rows = rows + "<tr><td>" + entry.ActivityType + "</td><td>" + entry.Description + "</td><td><a href='" + entry.Link + "'>Check on GitHub</a></td></tr>";
            });
            table.append(rows);
        }
    });

    function loadActivities() {
        return $.get('/api/activities');
    }

    function loadActivitiesAfter(activityId) {
        return $.get('/api/activities/' + activityId);
    }
})();