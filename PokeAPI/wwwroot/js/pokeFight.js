import { createCardInfo, getParam, getRandomNumber } from './modules/helpers.js'
import { getPokemonInfo } from './modules/poke.js'

$(async function () {
    function fillSelect() {
        let select = $('.fight__select')
        for (let i = 1; i <= 10; i++) {
            var option = $("<option>").text(i)
            select.append(option)
        }
    }

    function postFightResult(winPoke, losePoke) {
        fetch("/fight/stat", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                'winPokeId': parseInt(winPoke['id']),
                'losePokeId': parseInt(losePoke['id']),
                'countRounds': parseInt(round)
            })
        })
    }

    function reprintCards(pokemons) {
        $('.content__inner').empty()
        for (const pokemon of pokemons) {
            let card = createCardInfo(pokemon, true)
            if (pokemon['hp'] === 0) {
                card.css({
                    'opacity': '0.5',
                    'background-color': 'rgba(255, 0, 0, 0.3)',
                })
            }
            $('.content__inner').append(card)
        }
    }

    function fillFightHistory(attackingPoke, attackedPoke) {
        $('.fight__history').append(`<div><b>Раунд</b>: ${round} <b>${attackingPoke['name']}</b> attacked <b>${attackedPoke['name']}</b><div>`)
        if (attackedPoke['hp'] === 0) {
            $('.attack__btn, .fight__select').prop("disabled", true)
            $('.fight__history').append(`<div><b>Winner</b> ${attackingPoke['name']}<div>`)
            $('.modal__btn').prop('disabled', false)
            postFightResult(attackingPoke, attackedPoke)
        }
    }

    async function attack(attackingPoke, attackedPoke) {
        let response = await fetch(`/api/pokemon/fight/${attackingPoke['attackPower']}`, {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify(attackedPoke)
        });

        if (response.ok) {
            round++
            let updatedAttackedPoke = await response.json()
            attackedPoke['hp'] = updatedAttackedPoke['hp']
            fillFightHistory(attackingPoke, attackedPoke)
        }
    }

    let myPoke = await getPokemonInfo(getParam('myPokeId'))
    let enemyPoke = await getPokemonInfo(getParam('enemyPokeId'))
    let round = 0

    fillSelect()
    reprintCards([myPoke, enemyPoke])

    $('.back__btn').on('click', function (event) {
        window.history.replaceState({ page: 'pokeList' }, document.title, 'pokeList.html');
        window.close();
    })

    $('.attack__btn').on('click', async function (event) {
        $('.fast-fight').prop("disabled", true)

        let myNum = $('.fight__select').val()
        let enemyNum = (getRandomNumber(10) + 1)

        if (myNum % 2 === 0 && enemyNum % 2 === 0 || myNum % 2 !== 0 && enemyNum % 2 !== 0) {
            await attack(enemyPoke, myPoke)
        } else {
            await attack(myPoke, enemyPoke)
        }
        reprintCards([myPoke, enemyPoke])
    })

    $('.modal__btn').on('click', function (event) {
        $('.email__modal').modal('show')
        let winPoke = myPoke['hp'] !== 0 ? myPoke : enemyPoke
        let losePoke = myPoke['hp'] === 0 ? myPoke : enemyPoke
        $('#message-text').val(`${winPoke['name']} vs ${losePoke['name']}. Win: ${winPoke['name']}. Rounds: ${round}`)
    })

    $('.send__email').on('click', async function (event) {
        let email = $('.email__address').val()
        let message = $('.email__message').val()

        await fetch("/fight/stat/send-email", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                'email': email,
                'message': message
            })
        })
        $('.email__modal').modal('hide')
    })

    $('.fast-fight__btn').on('click', async function (event) {
        event.preventDefault()

        $('.attack__btn, .fast-fight, .fight__select').prop("disabled", true)
        let response = await fetch(`/api/fight/fast?myPokeId=${myPoke['id']}&enemyPokeId=${enemyPoke['id']}`, {
            method: "GET",
            headers: { "Accept": "application/json" }
        })

        if (response.ok) {
            let statistics = await response.json()
            for (let stat of statistics) {
                round = stat['round']
                fillFightHistory(stat['attackingPoke'], stat['attackedPoke'])
                if (stat['isMyPokeWin']) {
                    reprintCards([stat['attackingPoke'], stat['attackedPoke']])
                    myPoke['hp'] = stat['attackingPoke']['hp']
                    enemyPoke['hp'] = stat['attackedPoke']['hp']
                } else {
                    reprintCards([stat['attackedPoke'], stat['attackingPoke']])
                    myPoke['hp'] = stat['attackedPoke']['hp']
                    enemyPoke['hp'] = stat['attackingPoke']['hp']
                }
            }
        }
    })
})