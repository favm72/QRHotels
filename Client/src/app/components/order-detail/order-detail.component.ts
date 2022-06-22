import { Component, OnInit, ViewChild } from "@angular/core"
import { Router } from "@angular/router"
import { Subscription } from "rxjs"
import { OrderService } from "src/app/services/order.service"
import { ModalComponent } from "../utils/modal/modal.component"

@Component({
	selector: "app-order-detail",
	templateUrl: "./order-detail.component.html",
	styleUrls: ["./order-detail.component.css"],
})
export class OrderDetailComponent implements OnInit {
	order = null
	detail = []
	reasons = []

	@ViewChild("mApprove") mApprove: ModalComponent
	@ViewChild("mReject") mReject: ModalComponent
	@ViewChild("mFinalize") mFinalize: ModalComponent

	constructor(private router: Router, private orderService: OrderService) {
		if (orderService.selectedOrder == null) {
			this.router.navigate(["/admin/main"])
			return
		}

		this.order = orderService.selectedOrder

		if (this.order.atentionTime == null) {
			if (this.order.idProvider == 1) this.order.atentionTime = 20
			else if (this.order.idProvider == 2) this.order.atentionTime = 40
			else if (this.order.idProvider == 6) this.order.atentionTime = 25
			else if (this.order.idProvider == 3) this.order.atentionTime = 0
			else if (this.order.idProvider == 4) this.order.atentionTime = 0
			else if (this.order.idProvider == 9) this.order.atentionTime = 0
			else if (this.order.idProvider == 10) this.order.atentionTime = 0
			else this.order.atentionTime = 5
		}
		if (this.order.comment == null) {
			this.order.comment = ""
		}
		if (this.order.idReason == null) {
			this.order.idReason = -1
		}
	}

	async ngOnInit(): Promise<void> {
		await this.orderService.getDetail(true)
		this.detail = this.orderService.detail
		await this.orderService.getReasons()
		this.reasons = this.orderService.reasons
	}

	approveOpen() {
		this.mApprove.open()
	}
	rejectOpen() {
		this.mReject.open()
	}
	finalizeOpen() {
		this.mFinalize.open()
	}

	goToMain() {
		this.router.navigate(["/admin/main"])
	}

	goBack() {
		this.router.navigate(["/admin/main"])
	}

	async approve(): Promise<void> {
		this.orderService.selectedOrder.atentionTime = this.order.atentionTime
		this.orderService.selectedOrder.comment = this.order.comment
		await this.orderService.approve(() => {
			this.goToMain()
		})
	}

	async reject(): Promise<void> {
		this.orderService.selectedOrder.idReason = this.order.idReason
		this.orderService.selectedOrder.comment = this.order.comment
		await this.orderService.reject(() => {
			this.goToMain()
		})
	}

	async finalize(): Promise<void> {
		this.orderService.selectedOrder.comment = this.order.comment
		await this.orderService.finalize(() => {
			this.goToMain()
		})
	}
}
