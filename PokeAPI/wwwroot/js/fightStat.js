import { createStatPagination } from "./modules/createPagination.js"

$(async function () {
    async function getFightStatistics() {
        const response = await fetch("/fight/stat/data", {
            method: "GET",
            headers: { "Accept": "application/json" }
        })

        if (response.ok) {
            let result = await response.json()
            return result
        }
        return null
    }

    let stat = await getFightStatistics()
    createStatPagination(stat, '#pagination', '.table__tbody')

    $('.back__btn').on('click', function (event) {
        event.preventDefault()
        window.history.replaceState({ page: 'index' }, document.title, 'index.html');
        window.close();
    })
})