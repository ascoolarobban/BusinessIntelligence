// script file

let tabledata = document.getElementById('tabledata')

fetch("https://robinstorage.azurewebsites.net/api/GetDataFromTableStorage?code=kvtHgx5i9f6lkahRvXh8qYt6Gl5ua/287UoO2hnMDXfy8DJ/iF/TQQ==")
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

