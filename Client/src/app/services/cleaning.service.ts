import { Injectable } from "@angular/core"
import { INFO_CLEANING, ORDER_SAVE } from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

export interface ICleaningOrder {
	prefix: string
	number: string
	price: number
}

@Injectable({
	providedIn: "root",
})
export class CleaningService {
	constructor(private api: ApiService, private notif: NotificationService, private signal: SignalOrderService) {}

	async getOrderInfo(): Promise<any> {
		let result = null
		const req = new MyReq("Get Cleaning info")
		req.url = INFO_CLEANING
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}
	async saveOrder(params: ICleaningOrder, callback): Promise<void> {
		let order: any = {}
		order.phone = `+${params.prefix}${params.number}`
		order.idProvider = 16
		order.total = params.price
		order.quantity = 1

		let req = new MyReq("Save Cleaning Order")
		req.url = ORDER_SAVE
		req.model = order

		req.success = res => {
			this.notif.newOrder(16)
			let model = new OrderMessageModel()
			model.statusCode = "P"
			this.signal.sendOrder(model)

			localStorage.number = params.number
			localStorage.prefix = params.prefix

			callback()
		}
		await this.api.request(req)
	}
}
