// script file

let tabledata = document.getElementById('tabledata')

fetch("https://savetotablestorage.azurewebsites.net/api/GetDataFromTableStorage?code=3bzCg7fGnZoNczLiJuA3SfsffPsQOVawmr9aNOTV9JcoxQpKd3z/aA==")
    .then(res=> res.json())
    .then(data=>{
        console.log(data)
        for(let row of data){
            tabledata.innerHTML += `
        <tr>
        <td>${row.deviceId}</td>
        <td>${row.activity}</td>
        <td>${row.timestamp}</td>
 
        `
        }

    })

