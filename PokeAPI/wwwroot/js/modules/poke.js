//import { getRandomNumber } from "./helpers.js"
export { getPokemonList, getPokemonInfo, getRandomPokemon }

async function getPokemonList() {
    let response = await fetch("/api/pokemon/list", {
        method: "GET",
        headers: { "Accept": "application/json" }
    })

    if (response.ok) {
        let result = await response.json()
        return result
    }
    return null
}

async function getPokemonInfo(id) {
    let response = await fetch(`/api/pokemon/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })
    if (response.ok) {
        let result = await response.json()
        return result
    }
    return null
}

async function getRandomPokemon() {
    let response = await fetch(`/api/pokemon/random`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    })
    if (response.ok) {
        let result = await response.json()
        return result
    }
    return null
}

//async function getCount() {
//    let url = `https://pokeapi.co/api/v2/pokemon/`
//    let response = await fetch(url)

//    if (response.ok) {
//        let result = await response.json()
//        return result['count']
//    }
//    return 0
//}

//async function randomePokemon() {
//    let limit = await getCount()
//    let url = `https://pokeapi.co/api/v2/pokemon?limit=${limit}`
//    let response = await fetch(url)

//    if (response.ok) {
//        let result = await response.json()
//        const names = []
//        for (const name of result['results']) {
//            names.push(name['name'])
//        }
//        return await getPokemonInfo(names[getRandomNumber(limit)])
//    }
//}