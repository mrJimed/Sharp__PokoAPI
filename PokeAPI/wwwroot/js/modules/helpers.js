export { getRandomNumber, createCardInfo, getParam }

function getRandomNumber(max) {
    return Math.floor(Math.random() * max);
}

function getParam(paramName) {
    let urlParams = new URLSearchParams(window.location.search)
    let dataParam = urlParams.get(paramName)
    if (dataParam) {
        return JSON.parse(dataParam)
    }
    return null
}

function createCardInfo(poke, isFight = false) {
    return $(`
    <div class="card card--big ${isFight ? 'card--fight' : ''}">
        <img src="${poke['image']}" class="card-img-top" alt="${poke['name']}">
        <div class="card-body">
            <h5 class="card-title">${poke['name']}</h5>
            <p class="card-text">
                Hp: ${poke['hp']} <br/>
                Attack power: ${poke['attackPower']} <br/>
                Weight: ${poke['weight']} <br/>
                Height: ${poke['height']}
            </p>
            ${isFight ? '' : '<a href="/pokemon/fight" class="btn btn-primary fight__btn">Select</a>'}
        </div>
    </div>
    `)
}