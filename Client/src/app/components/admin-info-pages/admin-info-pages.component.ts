import { Component, OnInit, ViewChild } from "@angular/core"
import { NotificationService } from "src/app/services/notification.service"
import { ModalComponent } from "../utils/modal/modal.component"
import { faEdit, faSearch } from "@fortawesome/free-solid-svg-icons"
import { InfoService } from "src/app/services/info.service"
import { InfoPagesService } from "src/app/services/info-pages.service"

const initialHeadForm = () => ({
	name: { value: "", invalid: false },
})

const initialDetailForm = () => ({
	description: { value: "", invalid: false },
	title: { value: "", invalid: false },
	mapUrl: { value: "", invalid: false },
	linkUrl: { value: "", invalid: false },
	imageUrl: { value: "", invalid: false },
	orderNo: { value: 0, invalid: false },
	image64: { value: "", invalid: false },
	filename: { value: "", invalid: false },
	active: { value: true, invalid: false },
})

@Component({
	selector: "app-admin-info-pages",
	templateUrl: "./admin-info-pages.component.html",
	styleUrls: ["./admin-info-pages.component.css"],
})
export class AdminInfoPagesComponent implements OnInit {
	editIcon = faEdit
	searchIcon = faSearch
	hotels = []
	currentHotel = ""
	currentHead = null
	currentDetail = null
	heads = []
	detail = []
	formDetail = initialDetailForm()
	formHead = initialHeadForm()
	viewDetail = false

	@ViewChild("modalDetailForm") modalDetailForm: ModalComponent
	@ViewChild("modalHeadForm") modalHeadForm: ModalComponent
	constructor(private info: InfoService, private notif: NotificationService, private infoPage: InfoPagesService) {}

	async ngOnInit(): Promise<void> {
		this.hotels = await this.info.loadHotels()
	}

	get hotelDescription() {
		return this.hotels.find(h => h.id == this.currentHotel)?.description ?? ""
	}

	get filteredHeads() {
		if (this.viewDetail) {
			return this.heads.filter(h => h.id === this.currentHead.id)
		} else {
			return this.heads
		}
	}

	viewHead(head) {
		this.currentHead = head
		this.viewDetail = true
		this.loadDetail()
	}

	async loadHead() {
		this.detail = []
		if (this.currentHotel == "") {
			this.heads = []
		} else {
			const data = await this.infoPage.listHead(this.currentHotel)
			this.heads = data
		}
	}

	async loadDetail() {
		if (this.currentHead == null) {
			this.detail = []
		} else {
			this.detail = await this.infoPage.listDetailAdmin(+this.currentHead.id)
		}
	}

	async onChangeHotel() {
		this.loadHead()
	}

	closeDetail() {
		this.viewDetail = false
		this.currentDetail = null
		this.currentHead = null
		this.formDetail = initialDetailForm()
		this.detail = []
	}

	openDetailModal(detail) {
		this.currentDetail = detail
		if (this.currentDetail) {
			this.formDetail.active.value = detail.active
			this.formDetail.title.value = detail.title
			this.formDetail.description.value = detail.description ? decodeURIComponent(atob(detail.description)) : ""
			this.formDetail.orderNo.value = detail.orderNo
			this.formDetail.mapUrl.value = detail.mapUrl
			this.formDetail.linkUrl.value = detail.linkUrl
			this.formDetail.imageUrl.value = detail.imageUrl
		} else {
			this.formDetail = initialDetailForm()
		}
		this.modalDetailForm.open()
	}

	openHeadModal(head) {
		this.currentHead = head
		if (this.currentHead) {
			this.formHead.name.value = this.currentHead.name
		} else {
			this.formHead = initialHeadForm()
		}
		this.modalHeadForm.open()
	}

	async saveDetail() {
		const model = {
			title: this.formDetail.title.value,
			description: btoa(encodeURIComponent(this.formDetail.description.value)),
			orderNo: +this.formDetail.orderNo.value,
			image64: this.formDetail.image64.value,
			filename: this.formDetail.filename.value,
			active: this.formDetail.active.value,
			hotelCode: this.currentHotel,
			mapUrl: this.formDetail.mapUrl.value,
			linkUrl: this.formDetail.linkUrl.value,
			idInfoPage: this.currentHead?.id,
		}
		let result = false
		if (this.currentDetail == null) result = await this.infoPage.createDetail(model)
		else result = await this.infoPage.editDetail({ ...model, id: this.currentDetail.id })

		if (result) {
			this.notif.success(`${this.currentDetail == null ? "Registrado" : "Actualizado"} correctamente`)
			this.loadDetail()
			this.modalDetailForm.close()
			this.formDetail = initialDetailForm()
		}
	}

	async saveHead() {
		const model = {
			name: this.formHead.name.value,
			hotelCode: this.currentHotel,
		}
		let result = false
		if (this.currentHead == null) result = await this.infoPage.createHead(model)
		else result = await this.infoPage.editHead({ ...model, id: this.currentHead.id })

		if (result) {
			this.notif.success("Actualizado correctamente")
			this.loadHead()
			this.modalHeadForm.close()
			this.formHead = initialHeadForm()
		}
	}
}
