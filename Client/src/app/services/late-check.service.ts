import { Injectable } from "@angular/core"
import { ORDER_SAVE } from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

@Injectable({
	providedIn: "root",
})
export class LateCheckService {
	constructor(private api: ApiService, private notif: NotificationService, private signal: SignalOrderService) {}

	async saveOrder(prefix, number, lateCheck, callback): Promise<void> {
		let order: any = {}
		order.phone = `+${prefix}${number}`
		order.idProvider = 3
		order.idLateCheckout = lateCheck.id
		order.total = lateCheck.price
		order.quantity = 1

		let req = new MyReq("Save Order LateCheck")
		req.url = ORDER_SAVE
		req.model = order

		req.success = res => {
			this.notif.newOrder(3)
			let model = new OrderMessageModel()
			model.statusCode = "P"
			this.signal.sendOrder(model)

			localStorage.number = number
			localStorage.prefix = prefix

			callback()
		}
		await this.api.request(req)
	}
}
