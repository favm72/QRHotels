import { Injectable } from "@angular/core"
import { Router } from "@angular/router"
import { Subject } from "rxjs"
import {
	ORDER_APPROVE,
	ORDER_DETAIL,
	ORDER_DISCOUNT,
	ORDER_FINALIZE,
	ORDER_FINDBYID,
	ORDER_LIST,
	ORDER_LIST_BYCLIENT,
	ORDER_MARK_VIEWED,
	ORDER_NOTIFICATIONS_BYCLIENT,
	ORDER_REASONS,
	ORDER_REJECT,
	PRODUCT_MANAGE,
	PRODUCT_TOGGLE,
} from "../api/endpoints"
import { OrderMessageModel } from "../models/order"
import { ApiService, MyReq } from "./api.service"
import { NotificationService } from "./notification.service"
import { SignalOrderService } from "./signal-order.service"

@Injectable({
	providedIn: "root",
})
export class OrderService {
	orders = []
	selectedOrder = null
	detail = []
	reasons = []
	filters = {
		pageSize: 15,
		currentPage: 1,
		hotelCode: "",
		statusCode: "T",
		roomCode: "",
		lastName: "",
		firstName: "",
		idProvider: [],
	}
	onViewNotifClient: Subject<void> = new Subject<void>()

	constructor(private router: Router, private notif: NotificationService, private signalOrder: SignalOrderService, private api: ApiService) {}

	async sendSignal(statusCode: string) {
		if (this.selectedOrder == null) {
			console.log("selected order is null")
			return
		}
		let model = new OrderMessageModel()
		model.id = this.selectedOrder.id
		model.client = this.selectedOrder.client
		model.statusCode = statusCode
		await this.signalOrder.sendOrder(model)
	}

	async getOrders() {
		let req = new MyReq("get orders admin")
		req.url = ORDER_LIST
		req.model = { ...this.filters, currentPage: this.filters.currentPage - 1 }
		req.success = res => {
			this.orders = res.data
		}
		req.isAdmin = true
		await this.api.request(req)
	}

	async getProducts() {
		let products = []
		let req = new MyReq("get products admin")
		req.url = PRODUCT_MANAGE
		req.success = res => {
			products = res.data
		}
		req.isAdmin = true
		await this.api.request(req)
		return products
	}

	async setSelectedOrder(id: number, admin: boolean) {
		let req = new MyReq(`get detail idorder=${id} isadmin=${admin}`)
		req.url = ORDER_FINDBYID
		req.model = { id: id }
		req.success = res => {
			this.selectedOrder = res.data
		}
		req.isAdmin = admin
		await this.api.request(req)
	}

	async notifByClient() {
		let notifications = []
		let req = new MyReq("list notifications (bell) client")
		req.url = ORDER_NOTIFICATIONS_BYCLIENT
		req.success = res => {
			notifications = res.data
		}
		req.notify = false
		await this.api.request(req)
		return notifications
	}

	async markViewed(order) {
		let req = new MyReq(`mark viewed order id=${order.id}`)
		req.url = ORDER_MARK_VIEWED
		req.model = { id: order.id }
		req.success = res => {
			this.onViewNotifClient.next()
		}
		req.notify = false
		await this.api.request(req)
	}

	async toggleProduct(id: number) {
		let req = new MyReq(`activate/deact product id=${id}`)
		req.url = PRODUCT_TOGGLE
		req.model = { id: id }
		req.success = res => {
			console.log("actualizado")
		}
		req.isAdmin = true
		await this.api.request(req)
	}

	async getOrdersByClient() {
		let req = new MyReq(`get orders by client`)
		req.url = ORDER_LIST_BYCLIENT
		req.success = res => {
			this.orders = res.data
		}
		await this.api.request(req)
	}

	async getDetail(admin: boolean) {
		let req = new MyReq(`get order detail isAdmin=${admin}`)
		req.url = ORDER_DETAIL
		req.model = { id: this.selectedOrder.id }
		req.success = res => {
			this.detail = res.data
		}
		req.isAdmin = admin
		await this.api.request(req)
	}

	async getReasons() {
		let req = new MyReq(`get reasons`)
		req.url = ORDER_REASONS
		req.isAdmin = true
		req.success = res => {
			this.reasons = res.data
		}
		await this.api.request(req)
	}

	async approve(callback) {
		let req = new MyReq(`approve order id=${this.selectedOrder.id}`)
		req.url = ORDER_APPROVE
		req.model = {
			id: this.selectedOrder.id,
			atentionTime: this.selectedOrder.atentionTime,
			comment: this.selectedOrder.comment,
		}
		req.success = res => {
			this.detail = res.data
			this.router.navigate["/admin/main"]
			this.notif.success("Orden aprobada correctamente.")
			this.sendSignal("A")
			callback()
		}
		req.isAdmin = true
		await this.api.request(req)
	}

	async reject(callback) {
		if (this.selectedOrder.idReason <= 0) {
			this.notif.error("Debe seleccionar un motivo de rechazo.")
			return
		}
		let req = new MyReq(`reject order id=${this.selectedOrder.id}`)
		req.url = ORDER_REJECT
		req.model = {
			id: this.selectedOrder.id,
			comment: this.selectedOrder.comment,
			idReason: this.selectedOrder.idReason,
		}
		req.success = res => {
			this.detail = res.data
			this.notif.success("Orden rechazada correctamente.")
			this.sendSignal("R")
			callback()
		}
		req.isAdmin = true
		await this.api.request(req)
	}

	async finalize(callback) {
		let req = new MyReq(`finalize order id=${this.selectedOrder.id}`)
		req.url = ORDER_FINALIZE
		req.model = {
			id: this.selectedOrder.id,
			comment: this.selectedOrder.comment,
		}
		req.success = res => {
			this.detail = res.data
			this.notif.success("Orden finalizada correctamente.")
			this.sendSignal("F")
			callback()
		}
		req.isAdmin = true
		await this.api.request(req)
	}
}
