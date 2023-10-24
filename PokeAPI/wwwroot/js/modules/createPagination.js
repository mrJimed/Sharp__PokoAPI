import { getPokemonInfo } from "./poke.js";
export { createPagination, createStatPagination }

function createCard(pokemon) {
    return $(`
    <div class="card mb-3"">
        <div class="row g-0 card__inner">
            <div class="col-md-4">
                <a class="open__poce" href="pokemon/${pokemon['id']}" target="_blank">
                    <img src="${pokemon['image']}" class="img-fluid rounded-start" alt="${pokemon['name']}">
                </a>
            </div>
            <div class="col-md-8">
                <div class="card-body">
                    <a class="open__poce" href="pokemon/${pokemon['id']}" target="_blank">
                        <h5 class="card-title">${pokemon['name']}</h5>
                    </a>
                    <p class="card-text">
                        HP: ${pokemon['hp']} <br/>
                        Attack power: ${pokemon['attackPower']}
                    </p>
                    <button type="button" class="btn btn-dark fight__btn">Select</button>
                </div>
            </div>
        </div>
    </div>
    `)
}

function createPagination(sources, paginationId) {
    let container = $(paginationId)
    let options = {
        dataSource: sources,
        pageSize: 16,
        callback: function (response, pagination) {
            container.next().empty()
            $.each(response, async function (index, item) {
                let poke = await getPokemonInfo(item['id'])
                let card = createCard(poke)
                container.next().append(card)
            });
        }
    };
    container.pagination(options);
}

function createStatPagination(sources, paginationId, table) {
    let container = $(paginationId)
    let options = {
        dataSource: sources,
        pageSize: 14,
        callback: function (response, pagination) {
            $(table).empty()
            $.each(response, function (index, item) {
                let endTime = new Date(item['endTime'])
                $(table).append(`
                <tr>
                    <td>${item['winPokeId']}</td>
                    <td>${item['losePokeId']}</td>
                    <td>${item['countRounds']}</td>
                    <td>${endTime.toLocaleTimeString('ru-RU')} ${endTime.toLocaleDateString('ru-RU')}</td>
                </tr>
                `)
            });
        }
    };
    container.pagination(options);
}