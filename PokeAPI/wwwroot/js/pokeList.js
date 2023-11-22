import { getPokemonList, getRandomPokemon } from './modules/poke.js'
import { createPagination } from './modules/createPagination.js'

$(async function () {
    const CONTAINER_ID = '#pagination'
    const pokemons = await getPokemonList()

    createPagination(pokemons, CONTAINER_ID)

    $('.content__inner').on('click', '.open__poce', function () {
        window.history.replaceState({ page: 'pokeList' }, document.title, window.location.href)
    });

    $('.content__inner').on('click', '.fight__btn', async function () {
        let myPokeName = $(this).closest(".card").find(".card-title").text()
        let myPokeId = pokemons.find(poke => poke['name'] === myPokeName)['id']
        let enemyPoke = await getRandomPokemon()
        let url = '/pokemon/fight?myPokeId=' + JSON.stringify(myPokeId) + '&enemyPokeId=' + JSON.stringify(enemyPoke['id'])
        window.open(url, "_blank")
    });

    // $('.search__btn').on('input', function () {
    //     if (pokemons.length > 0) {
    //         let inputVal = $(this).val()
    //         const result = pokemons.filter(poke => poke['name'].startsWith(inputVal))
    //         createPagination(result, CONTAINER_ID)
    //     }
    // })

    $('.search__btn').on('click', function () {
        if (pokemons.length > 0) {
            let inputVal = $('.search__input').val()
            const result = pokemons.filter(poke => poke['name'].startsWith(inputVal))
            createPagination(result, CONTAINER_ID)
        }
    })

    $('.content__inner').on('click', '.save__btn', async function () {
        let pokemonName = $(this).closest(".card").find(".card-title").text()
        let pokemonId = pokemons.find(poke => poke['name'] === pokemonName)['id']

        let response = await fetch(`/api/ftp`, {
            method: "post",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                id: pokemonId,
            })
        })
    })
})