import { Injectable } from "@angular/core"
import * as signalR from "@aspnet/signalr"
import { Subject } from "rxjs"
import { baseUrl } from "src/constants"
import { OrderMessageModel } from "../models/order"

@Injectable({
	providedIn: "root",
})
export class SignalOrderService {
	private hubConnection: signalR.HubConnection

	onGetOrder: Subject<any> = new Subject<any>()
	onNewOrder: Subject<any> = new Subject<any>()

	async sendOrder(model: OrderMessageModel) {
		try {
			await this.hubConnection.invoke("Send", model)
		} catch (error) {
			console.log(error.message)
		}
	}

	assignListener() {
		this.hubConnection.on("getorder", data => {
			console.log({ signalr: "message received", data: data })
			// console.log("message received2")
			let localuser
			try {
				console.log("empezando local storage")
				// localuser= JSON.parse(localStorage.qrh)
				localuser = JSON.parse(localStorage.getItem("qrh") || localStorage.getItem("qrha"))
			} catch (ex) {
				console.log("error parse localstorage")
				console.log(ex)
			}
			// console.log("Json user")
			if (data.client == localuser.client && data.statusCode != "F") {
				this.onGetOrder.next(data)
			}
			// console.log("Evalua enviar a admin")
			if (data.statusCode == "P") {
				// console.log("Enviado a listar admin")
				this.onNewOrder.next()
			}
		})
	}

	async startConnection() {
		setInterval(async () => {
			if (this.hubConnection.state == signalR.HubConnectionState.Disconnected) {
				try {
					await this.hubConnection.start()
					console.log("Connection started")
				} catch (error) {
					console.log("Error Signal: " + error.message)
				}
			}
		}, 3000)
	}

	constructor() {
		this.hubConnection = new signalR.HubConnectionBuilder().withUrl(baseUrl + "orderhub").build()

		this.assignListener()
		this.startConnection()
	}
}
