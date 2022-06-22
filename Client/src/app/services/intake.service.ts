import { Injectable } from "@angular/core"
import { INTAKE_CANAGREE, INTAKE_DETAIL, INTAKE_LIST, INTAKE_MAIL, INTAKE_REASONS, ORDER_SAVE } from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

@Injectable({
	providedIn: "root",
})
export class IntakeService {
	currentIntake: any

	constructor(private api: ApiService, private notif: NotificationService, private signal: SignalOrderService) {}

	async loadIntakeList() {
		let intakeList = []
		let req = new MyReq("intake list")
		req.url = INTAKE_LIST
		req.success = res => {
			intakeList = res.data
		}
		await this.api.request(req)
		return intakeList
	}

	async loadIntakeDetail() {
		let detail = null
		let req = new MyReq(`intake detail ticket = ${this.currentIntake.ticket}`)
		req.url = INTAKE_DETAIL
		req.model = { ticket: this.currentIntake.ticket }
		req.success = res => {
			detail = res.data
		}
		await this.api.request(req)
		return detail
	}

	async canAgree() {
		let result = false
		let req = new MyReq(`intake can agree`)
		req.url = INTAKE_CANAGREE
		req.success = res => {
			result = res.data
		}
		await this.api.request(req)
		return result
	}

	async reasons() {
		let reasons = []
		let req = new MyReq("intake reasons")
		req.url = INTAKE_REASONS
		req.success = res => {
			reasons = res.data
		}
		await this.api.request(req)
		return reasons
	}

	async mail(model) {
		let result = false
		let req = new MyReq("intake mail")
		req.url = INTAKE_MAIL
		req.model = model
		req.success = () => {
			result = true
		}
		await this.api.request(req)
		return result
	}

	async saveOrder(description, reason, callback): Promise<void> {
		let order: any = {}
		order.phone = ""
		order.idProvider = 9
		order.total = 0
		order.quantity = 1
		order.comment = description
		order.idIntakeReason = reason

		let req = new MyReq("Save Order Mail Intake")
		req.url = ORDER_SAVE
		req.model = order

		req.success = res => {
			this.notif.newOrder(9)
			let model = new OrderMessageModel()
			model.statusCode = "P"
			this.signal.sendOrder(model)
			callback()
		}
		await this.api.request(req)
	}

	async saveAgreeOrder(callback): Promise<void> {
		let order: any = {}
		order.phone = ""
		order.idProvider = 10
		order.total = 0
		order.quantity = 1
		order.comment = "Usuario conforme con los consumos"

		let req = new MyReq("Save Agree Order")
		req.url = ORDER_SAVE
		req.model = order

		req.success = res => {
			this.notif.newOrder(10)
			let model = new OrderMessageModel()
			model.statusCode = "P"
			this.signal.sendOrder(model)
			callback()
		}
		await this.api.request(req)
	}
}
