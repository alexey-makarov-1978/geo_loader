
// to ease the solution I add base Api Url as global var here
const baseUrl = 'https://localhost:7104/';

document.addEventListener('DOMContentLoaded', () => {
    navigate('ip');

    document.getElementById('ip-form').addEventListener('submit', async (e) => {
        const ip = $('#ip').val().trim();
        const url = `${baseUrl}ip/location?ip=${encodeURIComponent(ip)}`;

        const resultPanel = $('#ip-results');
        search(e, url, resultPanel, ip);
    });

    document.getElementById('city-form').addEventListener('submit', async (e) => {
        const city = $('#city').val().trim();
        const url = `${baseUrl}city/locations?city=${encodeURIComponent(city)}`;

        const resultPanel = $('#city-results');

        search(e, url, resultPanel, city);
    });

    async function search(e, url, resultPanel, searchValue) {
        e.preventDefault();

        try {
            const response = await fetch(url);
            const data = await response.json();

            if (!response.ok) {
                const errorDescription = getErrorDescription(data);
                throw new Error(errorDescription);
            }

            showResult(data, resultPanel, searchValue);
        } catch (error) {
            resultPanel.html(`
                <div class="alert alert-danger" role="alert">
                    <strong>Error!</strong> ${error.message}
                </div>`);
        }
    }

    function getErrorDescription(data) {
        const title = data.title || 'Error';
        const errors = data.errors ? Object.values(data.errors).flat().join(', ') : 'An unknown error occurred.';
        return `${title}: ${errors}`;
    }

    function showResult(data, resultPanel, searchValue) {
        $(resultPanel).empty();
        $(resultPanel).show();

        if (data && data.length > 0) {
            $(resultPanel).append(`<h5>Location(s) for "${searchValue}"</h5>`);

            let tableHtml = `
        <table class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th>City</th>
                    <th>Country</th>
                    <th>Region</th>
                    <th>Postal</th>
                    <th>Organization</th>
                    <th>Latitude</th>
                    <th>Longitude</th>
                </tr>
            </thead>
            <tbody>`;

            data.forEach(location => {
                tableHtml += `
            <tr>
                <td>${location.city}</td>
                <td>${location.country}</td>
                <td>${location.region}</td>
                <td>${location.postal}</td>
                <td>${location.organization}</td>
                <td>${location.latitude}</td>
                <td>${location.longitude}</td>
            </tr>`;
            });

            tableHtml += `</tbody></table>`;

            $(resultPanel).append(tableHtml);
        } else {
            $(resultPanel).append(`<p>No locations found for "${searchValue}".</p>`);
        }
    }
});
function navigate(screen) {
    $('#content > div').addClass('hidden');
    $(`#screen-${screen}`).removeClass('hidden');
}
