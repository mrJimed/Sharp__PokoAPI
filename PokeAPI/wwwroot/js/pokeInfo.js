import { createCardInfo } from "./modules/helpers.js"
import { getPokemonInfo, getRandomPokemon } from "./modules/poke.js"

$(async function () {
    function getPokeId() {
        const url = window.location.href
        const pattern = /(\d+)$/
        const matches = url.match(pattern)
        if (matches) {
            const id = matches[1]
            return id
        }
        console.log(`URL: ${url} - id не найден`);
    }

    async function printPokemon(poke) {
        $('.center__cards').append(createCardInfo(poke))
    }

    let id = getPokeId()
    let poke = await getPokemonInfo(id)
    printPokemon(poke)

    $('.back__btn').on('click', function (event) {
        event.preventDefault()
        window.history.replaceState({ page: 'pokeList' }, document.title, 'pokeList.html');
        window.close();
    })

    $('.fight__btn').on('click', async function (event) {
        event.preventDefault()
        let myPoke = poke
        let enemyPoke = await getRandomPokemon()
        let url = '/pokemon/fight?myPokeId=' + JSON.stringify(myPoke['id']) + '&enemyPokeId=' + JSON.stringify(enemyPoke['id'])
        window.open(url, "_self")
    })
})