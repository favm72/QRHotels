import { Injectable } from "@angular/core"
import { ORDER_SAVE } from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

@Injectable({
	providedIn: "root",
})
export class AmmenityService {
	constructor(private api: ApiService, private notif: NotificationService, private signal: SignalOrderService) {}

	async saveOrder(prefix, number, products, callback): Promise<void> {
		let order: any = {}
		order.phone = `+${prefix}${number}`
		order.idProvider = 15
		order.products = products
		let req = new MyReq("Save Order Ammenities")
		req.url = ORDER_SAVE
		req.model = order

		req.success = res => {
			this.notif.newOrder(15)
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
